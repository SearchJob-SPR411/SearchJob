using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IVacancyService
    {
        Task<List<VacancyDto>> GetAllAsync();
        Task<VacancyDto?> GetByIdAsync(int id);
        Task<VacancyDto> CreateAsync(CreateVacancyDto dto);
        Task<bool> UpdateAsync(int id, UpdateVacancyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
