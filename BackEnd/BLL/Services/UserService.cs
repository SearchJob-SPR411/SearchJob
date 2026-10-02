using AutoMapper;
using BLL.DTO;
using DAL.Entity;
using DAL.Enums;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<UserEntity> _passwordHasher;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<UserEntity>();
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            if (dto.Role == UserRole.Admin)
            {
                throw new InvalidOperationException("Admin role cannot be assigned during registration");
            }

            var user = new UserEntity
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            var createdUser = await _userRepository.CreateAsync(user);

            return _mapper.Map<UserDto>(createdUser);
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;

            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            var userEntity = new UserEntity
            {
                Id = user.Id
            };

            await _userRepository.DeleteAsync(userEntity);

            return true;
        }
    }
}
