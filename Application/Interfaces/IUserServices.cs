using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserServices
    {
        public List<UserDto> GetAll();
        public UserDto? GetById(int id);
        public UserDto Add(CreateUserDto userDto);
        public void Update(UserDto userDto);
        public void Delete(int id);
        public bool EmailExists(string email);
        public User? Authenticate(string email, string password);
    }
}