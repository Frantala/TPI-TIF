using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;

namespace Application.Interfaces
{
    public interface IUserServices
    {
        public List<UserDto> GetAll();
        public UserDto GetById(int id);
        public void Add(CreateUserDto userDto);
        public void Update(UserDto userDto);
        public void Delete(int id);
    }
}