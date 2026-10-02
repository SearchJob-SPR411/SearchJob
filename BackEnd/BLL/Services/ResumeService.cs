using AutoMapper;
using BLL.DTO;
using DAL.Entity;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IResumeRepository _repository;
        private readonly IMapper _mapper;

        public ResumeService(
            IResumeRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
