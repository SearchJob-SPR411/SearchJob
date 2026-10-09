using DAL.Entity;

namespace DAL.Repository
{
    public interface IJobApplicationRepository
    {
        Task<List<JobApplicationEntity>> GetByUserIdAsync(int userId);
        Task<List<JobApplicationEntity>> GetByVacancyIdAsync(int vacancyId);
        Task<JobApplicationEntity?> GetByIdAsync(int id);
        Task<JobApplicationEntity> CreateAsync(JobApplicationEntity entity);
        Task UpdateAsync(JobApplicationEntity entity);
        Task<bool> ExistsAsync(int userId, int vacancyId);
    }
}
