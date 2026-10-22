using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task <List<User>> GetAllAsync();

        Task <User> GetByIdAsync(int id);

        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);
    }
}
