using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Services
{
    /// <summary>
    /// Hardcoded login service implementation for mock authentication (SPR-411 Issue #8).
    /// </summary>
    public class LoginService : ILoginService
    {
        private class HardcodedUser
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public UserDto User { get; set; } = null!;
        }

        private static readonly List<HardcodedUser> _mockUsers = new()
        {
            new HardcodedUser
            {
                Email = "admin@searchjob.com",
                Password = "password123",
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "SearchJob",
                    Email = "admin@searchjob.com",
                    PhoneNumber = "+380501234567",
                    CreatedAt = new DateTime(2026, 1, 15)
                }
            },
            new HardcodedUser
            {
                Email = "demo@searchjob.com",
                Password = "demo",
                User = new UserDto
                {
                    Id = 2,
                    FirstName = "Демо",
                    LastName = "Користувач",
                    Email = "demo@searchjob.com",
                    PhoneNumber = "+380671112233",
                    CreatedAt = new DateTime(2026, 2, 1)
                }
            },
            new HardcodedUser
            {
                Email = "hr@searchjob.com",
                Password = "hr123",
                User = new UserDto
                {
                    Id = 3,
                    FirstName = "Олена",
                    LastName = "Коваль",
                    Email = "hr@searchjob.com",
                    PhoneNumber = "+380931234567",
                    CreatedAt = new DateTime(2026, 3, 10)
                }
            }
        };

        public Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Task.FromResult(new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "Введіть email та пароль"
                });
            }

            var trimmedEmail = request.Email.Trim();
            var matchedUser = _mockUsers.FirstOrDefault(u =>
                u.Email.Equals(trimmedEmail, StringComparison.OrdinalIgnoreCase) &&
                u.Password == request.Password);

            if (matchedUser == null)
            {
                return Task.FromResult(new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "Невірний email або пароль"
                });
            }

            return Task.FromResult(new LoginResponseDto
            {
                IsSuccess = true,
                Message = "Успішний вхід",
                Token = $"mock-jwt-token-{Guid.NewGuid():N}",
                User = matchedUser.User
            });
        }
    }
}
