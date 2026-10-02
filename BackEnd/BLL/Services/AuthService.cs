using BLL.DTO;
using DAL.Entity;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly PasswordHasher<UserEntity> _passwordHasher;

        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<UserEntity>();
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };
        }
    }
}
