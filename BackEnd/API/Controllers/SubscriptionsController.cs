using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _service;

        public SubscriptionsController(ISubscriptionService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<SubscriptionDto>> GetMySubscription()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var subscription = await _service.GetByUserIdAsync(userId);

            if (subscription == null)
            {
                return NotFound();
            }

            return Ok(subscription);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{userId}/premium")]
        public async Task<IActionResult> ActivatePremium(int userId, DateTime expiresAt)
        {
            var activated = await _service.ActivatePremiumAsync(userId, expiresAt);

            if (!activated)
            {
                return NotFound(new
                {
                    message = "Subscription not found"
                });
            }

            return NoContent();
        }
    }
}