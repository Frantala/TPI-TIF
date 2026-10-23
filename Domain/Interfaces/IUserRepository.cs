using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        public List<User> GetAll();

        public User? GetById(int id);

        public User? GetByEmail(string email);

        public void Add(User user);

        public void Update(User user);

        public void Delete(int id);
    }
}
