using AutoMapper;
using BLL.DTO;
using DAL.Entity;
using DAL.Repository;

namespace BLL.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IResumeRepository _repository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IMapper _mapper;

        public ResumeService(
            IResumeRepository repository,
            IMapper mapper,
            ISubscriptionRepository subscriptionRepository,
            ISubscriptionService subscriptionService)
        {
            _repository = repository;
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
            _subscriptionService = subscriptionService;
        }

        public async Task<IEnumerable<ResumeDto>> GetAllAsync()
        {
            var resumes = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<ResumeDto>>(resumes);
        }

        public async Task<ResumeDto?> GetByIdAsync(int id)
        {
            var resume = await _repository.GetByIdAsync(id);

            if (resume == null)
                return null;

            return _mapper.Map<ResumeDto>(resume);
        }

        public async Task<IEnumerable<ResumeDto>> GetByUserIdAsync(int userId)
        {
            var resumes = await _repository.GetByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<ResumeDto>>(resumes);
        }

        public async Task<ResumeDto> CreateAsync(CreateResumeDto dto)
        {
            var subscription = await _subscriptionRepository.GetByUserIdAsync(dto.UserId);

            if (subscription == null)
            {
                throw new InvalidOperationException(
                    "User subscription was not found");
            }

            var subscriptionDto = _mapper.Map<SubscriptionDto>(subscription);

            var isPremiumActive = _subscriptionService.IsPremiumActive(subscriptionDto);

            if (!isPremiumActive)
            {
                var resumeCount = await _repository.CountByUserIdAsync(dto.UserId);

                if (resumeCount >= 2)
                {
                    throw new InvalidOperationException("Free users can have a maximum of 2 resumes");
                }
            }

            var resume = new ResumeEntity
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Summary = dto.Summary,
                Skills = dto.Skills,
                Experience = dto.Experience,
                Education = dto.Education,
                CreatedAt = DateTime.UtcNow
            };

            var createdResume = await _repository.CreateAsync(resume);

            return _mapper.Map<ResumeDto>(createdResume);
        }

        public async Task<bool> UpdateAsync(int id, UpdateResumeDto dto, int userId)
        {
            var resume = await _repository.GetByIdAsync(id);

            if (resume == null)
            {
                return false;
            }

            if (resume.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only update your own resume");
            }

            var updatedResume = new ResumeEntity
            {
                Id = resume.Id,
                UserId = resume.UserId,
                Title = dto.Title,
                Summary = dto.Summary,
                Skills = dto.Skills,
                Experience = dto.Experience,
                Education = dto.Education,
                CreatedAt = resume.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.UpdateAsync(updatedResume);

            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var resume = await _repository.GetByIdAsync(id);

            if (resume == null)
            {
                return false;
            }

            if (resume.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own resume");
            }

            var resumeToDelete = new ResumeEntity
            {
                Id = resume.Id
            };

            await _repository.DeleteAsync(resumeToDelete);

            return true;
        }
    }
}