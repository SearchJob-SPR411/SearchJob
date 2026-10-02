using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompanyDto>>> GetAll()
        {
            var companies = await _companyService.GetAllAsync();

            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetById(int id)
        {
            var company = await _companyService.GetByIdAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<CompanyDto>>> GetByUserId(int userId)
        {
            var companies = await _companyService.GetByUserIdAsync(userId);

            return Ok(companies);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> Create(CreateCompanyDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            dto.UserId = userId;

            try
            {
                var company = await _companyService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = company.Id },
                    company);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCompanyDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var updated = await _companyService.UpdateAsync(id, dto, userId);

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
                var deleted = await _companyService.DeleteAsync(
                    id,
                    userId);

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