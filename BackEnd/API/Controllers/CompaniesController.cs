using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> Create(CreateCompanyDto dto)
        {
            var company = await _companyService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = company.Id },
                company);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCompanyDto dto)
        {
            var updated = await _companyService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _companyService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}