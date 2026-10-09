using BLL.DTO;
using DAL.Enums;

namespace BLL.Services
{
    public interface IJobApplicationService
    {
        Task<JobApplicationDto> ApplyAsync(int userId, ApplyJobDto dto);
        Task<List<JobApplicationDto>> GetMyApplicationsAsync(int userId);
        Task<List<JobApplicationDto>> GetApplicationsByVacancyAsync(int vacancyId, int recruiterUserId);
        Task<bool> UpdateStatusAsync(int applicationId, ApplicationStatus newStatus, int recruiterUserId);
        Task<bool> HasAppliedAsync(int userId, int vacancyId);
        Task<JobApplicationDto?> GetByIdAsync(int applicationId, int userId);
    }
}
