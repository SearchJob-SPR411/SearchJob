using DAL.Entity;

namespace DAL.Repository
{
    public interface IFavoriteVacancyRepository
    {
        Task<List<FavoriteVacancyEntity>> GetByUserIdAsync(int userId);
        Task<FavoriteVacancyEntity?> GetByUserAndVacancyAsync(int userId, int vacancyId);
        Task<FavoriteVacancyEntity> AddAsync(FavoriteVacancyEntity entity);
        Task<bool> RemoveAsync(int userId, int vacancyId);
        Task<bool> IsFavoriteAsync(int userId, int vacancyId);
        Task<List<int>> GetFavoriteVacancyIdsAsync(int userId);
    }
}
