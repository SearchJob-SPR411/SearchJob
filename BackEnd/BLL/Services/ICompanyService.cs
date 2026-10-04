using BLL.DTO;

namespace BLL.Services
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> GetAllAsync();
        Task<CompanyDto?> GetByIdAsync(int id);
        Task<List<CompanyDto>> GetByUserIdAsync(int userId);
        Task<CompanyDto> CreateAsync(CreateCompanyDto dto);
        Task<bool> UpdateAsync(int id, UpdateCompanyDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
