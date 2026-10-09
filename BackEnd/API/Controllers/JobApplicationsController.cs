using BLL.DTO;
using BLL.Services;
using DAL.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/applications")]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationService _applicationService;

        public JobApplicationsController(IJobApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        private int? GetCurrentUserId(int? fallbackUserId = null)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }

            if (fallbackUserId.HasValue && fallbackUserId.Value > 0)
            {
                return fallbackUserId.Value;
            }

            return null;
        }

        [HttpPost("apply")]
        public async Task<ActionResult<JobApplicationDto>> Apply(
            [FromBody] ApplyJobDto dto,
            [FromQuery] int? userId = null)
        {
            var resolvedUserId = GetCurrentUserId(userId);
            if (!resolvedUserId.HasValue)
            {
                return Unauthorized(new { message = "Authentication required or provide userId." });
            }

            try
            {
                var result = await _applicationService.ApplyAsync(resolvedUserId.Value, dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<JobApplicationDto>>> GetMyApplications([FromQuery] int? userId = null)
        {
            var resolvedUserId = GetCurrentUserId(userId);
            if (!resolvedUserId.HasValue)
            {
                return Unauthorized(new { message = "Authentication required or provide userId." });
            }

            var applications = await _applicationService.GetMyApplicationsAsync(resolvedUserId.Value);
            return Ok(applications);
        }

        [HttpGet("vacancy/{vacancyId}")]
        public async Task<ActionResult<List<JobApplicationDto>>> GetByVacancy(
            int vacancyId,
            [FromQuery] int? recruiterUserId = null)
        {
            var resolvedUserId = GetCurrentUserId(recruiterUserId);
            if (!resolvedUserId.HasValue)
            {
                return Unauthorized(new { message = "Authentication required or provide recruiterUserId." });
            }

            try
            {
                var applications = await _applicationService.GetApplicationsByVacancyAsync(vacancyId, resolvedUserId.Value);
                return Ok(applications);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPut("{id}/status")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateApplicationStatusDto dto,
            [FromQuery] int? recruiterUserId = null)
        {
            var resolvedUserId = GetCurrentUserId(recruiterUserId);
            if (!resolvedUserId.HasValue)
            {
                return Unauthorized(new { message = "Authentication required or provide recruiterUserId." });
            }

            try
            {
                var updated = await _applicationService.UpdateStatusAsync(id, dto.Status, resolvedUserId.Value);
                if (!updated)
                {
                    return NotFound(new { message = "Application not found." });
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("check/{vacancyId}")]
        public async Task<ActionResult<bool>> HasApplied(
            int vacancyId,
            [FromQuery] int? userId = null)
        {
            var resolvedUserId = GetCurrentUserId(userId);
            if (!resolvedUserId.HasValue)
            {
                return Ok(false);
            }

            var applied = await _applicationService.HasAppliedAsync(resolvedUserId.Value, vacancyId);
            return Ok(applied);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplicationDto>> GetById(
            int id,
            [FromQuery] int? userId = null)
        {
            var resolvedUserId = GetCurrentUserId(userId);
            if (!resolvedUserId.HasValue)
            {
                return Unauthorized(new { message = "Authentication required or provide userId." });
            }

            try
            {
                var application = await _applicationService.GetByIdAsync(id, resolvedUserId.Value);
                if (application == null)
                {
                    return NotFound();
                }

                return Ok(application);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
