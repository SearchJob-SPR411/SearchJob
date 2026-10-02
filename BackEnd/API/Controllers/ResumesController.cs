using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumesController : ControllerBase
    {
        private readonly IResumeService _service;

        public ResumesController(IResumeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResumeDto>>> GetAll()
        {
            var resumes = await _service.GetAllAsync();

            return Ok(resumes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResumeDto>> GetById(int id)
        {
            var resume = await _service.GetByIdAsync(id);

            if (resume == null)
                return NotFound();

            return Ok(resume);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ResumeDto>>> GetByUserId(int userId)
        {
            var resumes = await _service.GetByUserIdAsync(userId);

            return Ok(resumes);
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPost]
        public async Task<ActionResult<ResumeDto>> Create(CreateResumeDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            dto.UserId = userId;

            var resume = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = resume.Id },
                resume);
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateResumeDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var updated = await _service.UpdateAsync(id, dto, userId);

                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var deleted = await _service.DeleteAsync(id, userId);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}