using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> Create(CreateVacancyDto dto)
        {
            var vacancy = await _vacancyService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = vacancy.Id },
                vacancy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateVacancyDto dto)
        {
            var updated = await _vacancyService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _vacancyService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}