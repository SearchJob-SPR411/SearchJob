using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IResumeService
    {
        Task<IEnumerable<ResumeDto>> GetAllAsync();
        Task<ResumeDto?> GetByIdAsync(int id);
        Task<IEnumerable<ResumeDto>> GetByUserIdAsync(int userId);
        Task<ResumeDto> CreateAsync(CreateResumeDto dto);
        Task<bool> UpdateAsync(int id, UpdateResumeDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
