using DAL.Entity;

namespace DAL.Repository
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetAllAsync();
        Task<UserEntity?> GetByIdAsync(int id);
        Task<UserEntity?> GetByEmailAsync(string email);
        Task<UserEntity> CreateAsync(UserEntity user);
        Task UpdateAsync(UserEntity user);
        Task DeleteAsync(UserEntity user);
    }
}