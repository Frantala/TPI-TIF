using Application.Dto;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService : IUserServices
    {
        private readonly IUserRepository _userRepository;

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
                Name = $"{user.Name}",
                Email = user.Email,
                
            }).ToList();
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            return new UserDto
            {
                Id = user.Id,
                Name = $"{user.Name}",
                Email = user.Email,
                
            };
        }

        public async Task AddAsync(CreateUserDto userDto)
        {
            var user = new Domain.Entities.User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                
            };

            await _userRepository.AddAsync(user);
        }

        public async Task UpdateAsync(UserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(userDto.Id);

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            await _userRepository.DeleteAsync(id);
        }
    }
}