using AutoMapper;
using BLL.DTO;
using DAL.Entity;
using DAL.Enums;
using DAL.Repository;

namespace BLL.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IResumeRepository _resumeRepository;
        private readonly IMapper _mapper;

        public JobApplicationService(
            IJobApplicationRepository applicationRepository,
            IVacancyRepository vacancyRepository,
            IResumeRepository resumeRepository,
            IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _vacancyRepository = vacancyRepository;
            _resumeRepository = resumeRepository;
            _mapper = mapper;
        }

        public async Task<JobApplicationDto> ApplyAsync(int userId, ApplyJobDto dto)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(dto.VacancyId);
            if (vacancy == null)
            {
                throw new KeyNotFoundException("Vacancy not found");
            }

            var alreadyApplied = await _applicationRepository.ExistsAsync(userId, dto.VacancyId);
            if (alreadyApplied)
            {
                throw new InvalidOperationException("You have already applied for this vacancy");
            }

            if (dto.ResumeId.HasValue)
            {
                var resume = await _resumeRepository.GetByIdAsync(dto.ResumeId.Value);
                if (resume == null || resume.UserId != userId)
                {
                    throw new ArgumentException("Invalid resume selected");
                }
            }

            var entity = new JobApplicationEntity
            {
                UserId = userId,
                VacancyId = dto.VacancyId,
                ResumeId = dto.ResumeId,
                CoverLetter = dto.CoverLetter,
                Status = ApplicationStatus.Pending,
                AppliedAt = DateTime.UtcNow
            };

            var created = await _applicationRepository.CreateAsync(entity);
            var reloaded = await _applicationRepository.GetByIdAsync(created.Id);

            return _mapper.Map<JobApplicationDto>(reloaded ?? created);
        }

        public async Task<List<JobApplicationDto>> GetMyApplicationsAsync(int userId)
        {
            var list = await _applicationRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<JobApplicationDto>>(list);
        }

        public async Task<List<JobApplicationDto>> GetApplicationsByVacancyAsync(int vacancyId, int recruiterUserId)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                throw new KeyNotFoundException("Vacancy not found");
            }

            if (vacancy.UserId != recruiterUserId)
            {
                throw new UnauthorizedAccessException("You can only view applications for your own vacancies");
            }

            var list = await _applicationRepository.GetByVacancyIdAsync(vacancyId);
            return _mapper.Map<List<JobApplicationDto>>(list);
        }

        public async Task<bool> UpdateStatusAsync(int applicationId, ApplicationStatus newStatus, int recruiterUserId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                return false;
            }

            if (application.Vacancy?.UserId != recruiterUserId)
            {
                throw new UnauthorizedAccessException("You can only manage applications for your own vacancies");
            }

            application.Status = newStatus;
            application.UpdatedAt = DateTime.UtcNow;

            await _applicationRepository.UpdateAsync(application);
            return true;
        }

        public async Task<bool> HasAppliedAsync(int userId, int vacancyId)
        {
            return await _applicationRepository.ExistsAsync(userId, vacancyId);
        }

        public async Task<JobApplicationDto?> GetByIdAsync(int applicationId, int userId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                return null;
            }

            // Allowed for either the applicant or the vacancy owner
            if (application.UserId != userId && application.Vacancy?.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have access to this application");
            }

            return _mapper.Map<JobApplicationDto>(application);
        }
    }
}
