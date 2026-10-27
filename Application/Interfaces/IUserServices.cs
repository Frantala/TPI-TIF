using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserServices
    {
        Task <List<UserDto>> GetAllAsync();
        Task <UserDto?> GetByIdAsync(int id);
        Task <UserDto> AddAsync(CreateUserDto userDto);
        Task UpdateAsync(UserDto userDto);
        Task DeleteAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> AuthenticateAsync(string email, string password);

    }
}