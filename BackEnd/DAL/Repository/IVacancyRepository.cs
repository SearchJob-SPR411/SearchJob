using DAL.Entity;

namespace DAL.Repository
{
    public interface IVacancyRepository
    {
        Task<List<VacancyEntity>> GetAllAsync();
        Task<VacancyEntity?> GetByIdAsync(int id);
        Task<VacancyEntity> CreateAsync(VacancyEntity vacancy);
        Task UpdateAsync(VacancyEntity vacancy);
        Task DeleteAsync(VacancyEntity vacancy);
    }
}