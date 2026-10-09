using BLL.DTO;

namespace BLL.Services
{
    public interface IFavoriteVacancyService
    {
        Task<List<VacancyDto>> GetFavoritesAsync(int userId);
        Task<bool> AddToFavoritesAsync(int userId, int vacancyId);
        Task<bool> RemoveFromFavoritesAsync(int userId, int vacancyId);
        Task<bool> IsFavoriteAsync(int userId, int vacancyId);
        Task<List<int>> GetFavoriteVacancyIdsAsync(int userId);
    }
}
