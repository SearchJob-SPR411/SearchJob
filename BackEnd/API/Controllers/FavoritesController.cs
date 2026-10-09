using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteVacancyService _favoriteService;

        public FavoritesController(IFavoriteVacancyService favoriteService)
        {
            _favoriteService = favoriteService;
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

        [HttpGet]
        public async Task<ActionResult<List<VacancyDto>>> GetFavorites([FromQuery] int? userId = null)
        {
            var resolvedUserId = GetCurrentUserId(userId);
            if (!resolvedUserId.HasValue)
            {
                return Unauthorized(new { message = "Authentication required or provide userId." });
            }

            var favorites = await _favoriteService.GetFavoritesAsync(resolvedUserId.Value);
            return Ok(favorites);
        }

        [HttpGet("ids")]
        public async Task<ActionResult<List<int>>> GetFavoriteIds([FromQuery] int? userId = null)
        {
            var resolvedUserId = GetCurrentUserId(userId);
            if (!resolvedUserId.HasValue)
            {
                return Unauthorized(new { message = "Authentication required or provide userId." });
            }

            var ids = await _favoriteService.GetFavoriteVacancyIdsAsync(resolvedUserId.Value);
            return Ok(ids);
        }

        [HttpGet("check/{vacancyId}")]
        public async Task<ActionResult<bool>> IsFavorite(int vacancyId, [FromQuery] int? userId = null)
        {
            var resolvedUserId = GetCurrentUserId(userId);
            if (!resolvedUserId.HasValue)
            {
                return Ok(false);
            }

            var isFav = await _favoriteService.IsFavoriteAsync(resolvedUserId.Value, vacancyId);
            return Ok(isFav);
        }

        [HttpPost("{vacancyId}")]
        public async Task<IActionResult> AddToFavorites(int vacancyId, [FromQuery] int? userId = null)
        {
            var resolvedUserId = GetCurrentUserId(userId);
            if (!resolvedUserId.HasValue)
            {
                return Unauthorized(new { message = "Authentication required or provide userId." });
            }

            var success = await _favoriteService.AddToFavoritesAsync(resolvedUserId.Value, vacancyId);
            if (!success)
            {
                return NotFound(new { message = "Vacancy not found." });
            }

            return Ok(new { message = "Added to favorites." });
        }

        [HttpDelete("{vacancyId}")]
        public async Task<IActionResult> RemoveFromFavorites(int vacancyId, [FromQuery] int? userId = null)
        {
            var resolvedUserId = GetCurrentUserId(userId);
            if (!resolvedUserId.HasValue)
            {
                return Unauthorized(new { message = "Authentication required or provide userId." });
            }

            var success = await _favoriteService.RemoveFromFavoritesAsync(resolvedUserId.Value, vacancyId);
            if (!success)
            {
                return NotFound(new { message = "Vacancy was not in favorites." });
            }

            return Ok(new { message = "Removed from favorites." });
        }
    }
}
