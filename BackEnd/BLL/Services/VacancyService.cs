using AutoMapper;
using BLL.DTO;
using DAL.Entity;
using DAL.Repository;

namespace BLL.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IMapper _mapper;

        public VacancyService(
            IVacancyRepository vacancyRepository,
            IMapper mapper)
        {
            _vacancyRepository = vacancyRepository;
            _mapper = mapper;
        }

        public async Task<List<VacancyDto>> GetAllAsync()
        {
            var vacancies = await _vacancyRepository.GetAllAsync();

            return _mapper.Map<List<VacancyDto>>(vacancies);
        }

        public async Task<VacancyDto?> GetByIdAsync(int id)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(id);

            if (vacancy == null)
            {
                return null;
            }

            return _mapper.Map<VacancyDto>(vacancy);
        }

        public async Task<List<VacancyDto>> GetByCompanyIdAsync(int companyId)
        {
            var vacancies = await _vacancyRepository.GetByCompanyIdAsync(companyId);
            return _mapper.Map<List<VacancyDto>>(vacancies);
        }

        public async Task<VacancyDto> CreateAsync(CreateVacancyDto dto)
        {
            var vacancy = new VacancyEntity
            {
                Title = dto.Title,
                CompanyId = dto.CompanyId,
                Location = dto.Location,
                EmploymentType = dto.EmploymentType,
                WorkFormat = dto.WorkFormat,
                SalaryMin = dto.SalaryMin,
                SalaryMax = dto.SalaryMax,
                Description = dto.Description,
                Status = dto.Status,
                UserId = dto.UserId
            };

            var createdVacancy = await _vacancyRepository.CreateAsync(vacancy);

            return _mapper.Map<VacancyDto>(createdVacancy);
        }

        public async Task<bool> UpdateAsync(int id, UpdateVacancyDto dto, int userId)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(id);

            if (vacancy == null)
            {
                return false;
            }

            if (vacancy.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only update your own vacancies");
            }

            vacancy.Title = dto.Title;
            vacancy.CompanyId = dto.CompanyId;
            vacancy.Location = dto.Location;
            vacancy.EmploymentType = dto.EmploymentType;
            vacancy.WorkFormat = dto.WorkFormat;
            vacancy.SalaryMin = dto.SalaryMin;
            vacancy.SalaryMax = dto.SalaryMax;
            vacancy.Description = dto.Description;
            vacancy.Status = dto.Status;
            vacancy.UpdatedAt = DateTime.UtcNow;

            await _vacancyRepository.UpdateAsync(vacancy);

            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(id);

            if (vacancy == null)
            {
                return false;
            }

            if (vacancy.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own vacancies");
            }

            var vacancyEntity = new VacancyEntity
            {
                Id = vacancy.Id
            };

            await _vacancyRepository.DeleteAsync(vacancyEntity);

            return true;
        }
    }
}
