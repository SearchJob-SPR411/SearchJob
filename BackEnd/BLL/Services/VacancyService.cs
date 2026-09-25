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

        public async Task<VacancyDto> CreateAsync(CreateVacancyDto dto)
        {
            var vacancy = new VacancyEntity
            {
                Title = dto.Title,
                CompanyName = dto.CompanyName,
                Location = dto.Location,
                EmploymentType = dto.EmploymentType,
                WorkFormat = dto.WorkFormat,
                SalaryMin = dto.SalaryMin,
                SalaryMax = dto.SalaryMax,
                Description = dto.Description,
                IsActive = dto.IsActive,
                UserId = dto.UserId
            };

            var createdVacancy =
                await _vacancyRepository.CreateAsync(vacancy);

            return _mapper.Map<VacancyDto>(createdVacancy);
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateVacancyDto dto)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(id);

            if (vacancy == null)
            {
                return false;
            }

            vacancy.Title = dto.Title;
            vacancy.CompanyName = dto.CompanyName;
            vacancy.Location = dto.Location;
            vacancy.EmploymentType = dto.EmploymentType;
            vacancy.WorkFormat = dto.WorkFormat;
            vacancy.SalaryMin = dto.SalaryMin;
            vacancy.SalaryMax = dto.SalaryMax;
            vacancy.Description = dto.Description;
            vacancy.IsActive = dto.IsActive;
            vacancy.UpdatedAt = DateTime.UtcNow;

            await _vacancyRepository.UpdateAsync(vacancy);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(id);

            if (vacancy == null)
            {
                return false;
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
