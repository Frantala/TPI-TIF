using Application.Dto;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            }).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<UserDto> AddAsync(CreateUserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name.Trim(),
                Email = userDto.Email.Trim(),
                Role = "user"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);
            await _userRepository.AddAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email.Trim()) is not null;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
{
    var user = await _userRepository.GetByEmailAsync(email.Trim());

    if (user is null)
    {
        Console.WriteLine("RESULTADO: Usuario no encontrado.");
        return null;
    }

    Console.WriteLine($"RESULTADO: Usuario encontrado: {user.Email}");
    Console.WriteLine($"RESULTADO: Rol: {user.Role}");

    var result = _passwordHasher.VerifyHashedPassword(
        user,
        user.PasswordHash,
        password
    );

    Console.WriteLine($"RESULTADO: Verificación: {result}");

    if (result == PasswordVerificationResult.Failed)
        return null;

    return user;
}
        public async Task UpdateAsync(UserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(userDto.Id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            await _userRepository.DeleteAsync(id);

        }
        public Task<UserDto?> PromoteToAdminAsync(int id) =>
            ChangeRoleAsync(id, "user", "Admin");

        public Task<UserDto?> DemoteAdminToUserAsync(int id) =>
            ChangeRoleAsync(id, "Admin", "user");

        public Task<UserDto?> PromoteAdminToSuperAdminAsync(int id) =>
            ChangeRoleAsync(id, "Admin", "SuperAdmin");

        private async Task<UserDto?> ChangeRoleAsync(
            int id,
            string expectedRole,
            string newRole)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null ||
                !string.Equals(user.Role, expectedRole, StringComparison.Ordinal))
            {
                return null;
            }

            user.Role = newRole;
            await _userRepository.UpdateAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}