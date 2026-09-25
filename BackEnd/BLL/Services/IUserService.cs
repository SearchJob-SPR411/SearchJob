using BLL.DTO;

namespace BLL.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<bool> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteAsync(int id);
    }
}