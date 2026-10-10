using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IResumeRepository
    {
        Task<IEnumerable<ResumeEntity>> GetAllAsync();
        Task<ResumeEntity?> GetByIdAsync(int id);
        Task<IEnumerable<ResumeEntity>> GetByUserIdAsync(int userId);
        Task<int> CountByUserIdAsync(int userId);
        Task<ResumeEntity> CreateAsync(ResumeEntity resume);
        Task UpdateAsync(ResumeEntity resume);
        Task DeleteAsync(ResumeEntity resume);
    }
}
