using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacanciesController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;

        public VacanciesController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vacancies = await _vacancyService.GetAllAsync();

            return Ok(vacancies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vacancy = await _vacancyService.GetByIdAsync(id);

            if (vacancy == null)
            {
                return NotFound();
            }

            return Ok(vacancy);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var vacancies = await _vacancyService.GetByCompanyIdAsync(companyId);
            return Ok(vacancies);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateVacancyDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            dto.UserId = userId;

            var vacancy = await _vacancyService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = vacancy.Id },
                vacancy);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateVacancyDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var updated = await _vacancyService.UpdateAsync(
                    id,
                    dto,
                    userId);

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

        [Authorize(Roles = "Recruiter")]
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
                var deleted = await _vacancyService.DeleteAsync(id, userId);

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