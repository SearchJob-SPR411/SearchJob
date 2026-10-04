using DAL.Entity;
using DAL.Enums;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;

namespace API.Seed
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userRepository = services.GetRequiredService<IUserRepository>();

            var existingAdmin = await userRepository.GetByEmailAsync("admin@gmail.com");

            if (existingAdmin != null)
            {
                return;
            }

            var admin = new UserEntity
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@gmail.com",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };

            var passwordHasher = new PasswordHasher<UserEntity>();

            admin.PasswordHash = passwordHasher.HashPassword(admin,"qwerty12345");

            await userRepository.CreateAsync(admin);
        }
    }
}