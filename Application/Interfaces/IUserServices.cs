using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;

namespace Application.Interfaces
{
    public interface IUserServices
    {
        Task <List<UserDto>> GetAllAsync();
        Task <UserDto> GetByIdAsync(int id);
        Task AddAsync(CreateUserDto userDto);
        Task UpdateAsync(UserDto userDto);
        Task DeleteAsync(int id);
    }
}