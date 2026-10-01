using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult<ResumeDto>> Create(CreateResumeDto dto)
        {
            var resume = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = resume.Id },
                resume);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateResumeDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}