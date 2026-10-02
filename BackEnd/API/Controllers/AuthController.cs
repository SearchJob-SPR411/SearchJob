using System.Threading.Tasks;
using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// Mock/Hardcoded user login endpoint.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "Некоректні дані запиту"
                });
            }

            var result = await _loginService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Returns test credentials for demonstration purposes.
        /// </summary>
        [HttpGet("test-accounts")]
        public IActionResult GetTestAccounts()
        {
            var accounts = new[]
            {
                new { Email = "admin@searchjob.com", Password = "password123", Role = "Admin" },
                new { Email = "demo@searchjob.com", Password = "demo", Role = "Демо користувач" },
                new { Email = "hr@searchjob.com", Password = "hr123", Role = "HR / Роботодавець" }
            };

            return Ok(accounts);
        }
    }
}
