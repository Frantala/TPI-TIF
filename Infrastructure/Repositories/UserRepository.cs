using System.Data;
using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<List<User>> GetAllAsync()
        {
            return await Context.Users.OrderBy(user => user.Id).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await Context.Users
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public override async Task AddAsync(User user)
        {
            var strategy = Context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

                if (!await Context.Users.AnyAsync())
                {
                    await ResetIdentityAsync();
                }

                await Context.Users.AddAsync(user);
                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
            });
        }

        public override async Task UpdateAsync(User user)
        {
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            var strategy = Context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

                var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    Context.Users.Remove(user);
                    await Context.SaveChangesAsync();

                    if (!await Context.Users.AnyAsync())
                    {
                        await ResetIdentityAsync();
                    }
                }

                await transaction.CommitAsync();
            });
        }

        private async Task ResetIdentityAsync()
        {
            await Context.Database.ExecuteSqlRawAsync(
                "DBCC CHECKIDENT ('Users', RESEED, 0)");
        }
    }
}