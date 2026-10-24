using System.Data;
using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.OrderBy(user => user.Id).ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(u => u.Email == email);
        }

        public void Add(User user)
        {
            using var transaction = _context.Database.BeginTransaction(
                IsolationLevel.Serializable);
            if (!_context.Users.Any())
            {
                ResetIdentity();
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            transaction.Commit();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var transaction = _context.Database.BeginTransaction(
                IsolationLevel.Serializable);
            var user = _context.Users.FirstOrDefault(user => user.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();

                if (!_context.Users.Any())
                {
                    ResetIdentity();
                }
            }

            transaction.Commit();
        }

        private void ResetIdentity()
        {
            _context.Database.ExecuteSqlRaw(
                "DBCC CHECKIDENT ('Users', RESEED, 0)");
        }
    }
}