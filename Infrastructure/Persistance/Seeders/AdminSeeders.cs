
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

            var existingSuperAdmin = await context.Users
                .Where(user => user.Role == "SuperAdmin")
                .FirstOrDefaultAsync();

            var hasher = new PasswordHasher<User>();

            if (existingSuperAdmin is not null)
            {
                if (string.Equals(
                        existingSuperAdmin.Email,
                        email,
                        StringComparison.OrdinalIgnoreCase))
                {
                    var verificationResult = hasher.VerifyHashedPassword(
                        existingSuperAdmin,
                        existingSuperAdmin.PasswordHash,
                        password);

                    if (verificationResult == PasswordVerificationResult.Failed)
                    {
                        existingSuperAdmin.PasswordHash = hasher.HashPassword(
                            existingSuperAdmin,
                            password);
                        await context.SaveChangesAsync();
                        logger.LogInformation(
                            "Configured SuperAdmin password synchronized.");
                    }
                    else
                    {
                        logger.LogInformation(
                            "Admin seeding skipped: the configured SuperAdmin already exists.");
                    }
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
