using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<CompanyEntity>> GetAllAsync();
        Task<CompanyEntity?> GetByIdAsync(int id);
        Task<IEnumerable<CompanyEntity>> GetByUserIdAsync(int userId);
        Task<CompanyEntity> CreateAsync(CompanyEntity company);
        Task UpdateAsync(CompanyEntity company);
        Task DeleteAsync(CompanyEntity company);
    }
}
