
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistance.Seeders
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            string email,
            string password,
            ILogger logger)
        {
            email = email.Trim();

            var existingSuperAdminEmail = await context.Users
                .Where(user => user.Role == "SuperAdmin")
                .Select(user => user.Email)
                .FirstOrDefaultAsync();

            if (existingSuperAdminEmail is not null)
            {
                if (string.Equals(
                        existingSuperAdminEmail,
                        email,
                        StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogInformation(
                        "Admin seeding skipped: the configured SuperAdmin already exists. Its password was not changed.");
                }
                else
                {
                    logger.LogWarning(
                        "Admin seeding skipped: a different SuperAdmin already exists. The configured account was not created.");
                }

                return;
            }

            bool emailExists = await context.Users
                .AnyAsync(u => u.Email == email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "El email configurado para el SuperAdmin " +
                    "ya pertenece a otro usuario."
                );
            }

            var admin = new User
            {
                Name = "SuperAdmin",
                Email = email,
                Role = "SuperAdmin"
            };

            var hasher = new PasswordHasher<User>();

            admin.PasswordHash = hasher.HashPassword(
                admin,
                password
            );

            context.Users.Add(admin);

            await context.SaveChangesAsync();
            logger.LogInformation("Initial SuperAdmin account created.");
        }
    }
}
