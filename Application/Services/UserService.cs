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

        public List<UserDto> GetAll()
        {
            var users = _userRepository.GetAll();

            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            }).ToList();
        }

        public UserDto? GetById(int id)
        {
            var user = _userRepository.GetById(id);

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

        public UserDto Add(CreateUserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name.Trim(),
                Email = userDto.Email.Trim(),
                Role = "user"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);
            _userRepository.Add(user);
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public bool EmailExists(string email)
        {
            return _userRepository.GetByEmail(email.Trim()) is not null;
        }

        public User? Authenticate(string email, string password)
        {
            var user = _userRepository.GetByEmail(email.Trim());
            if (user is null)
            {
                return null;
            }

            if (_passwordHasher.VerifyHashedPassword(
                    user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;
        }

        public void Update(UserDto userDto)
        {
            var user = _userRepository.GetById(userDto.Id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            

            _userRepository.Update(user);
        }

        public void Delete(int id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _userRepository.Delete(id);
        }
    }
}