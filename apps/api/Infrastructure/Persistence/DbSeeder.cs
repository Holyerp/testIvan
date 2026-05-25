using Microsoft.EntityFrameworkCore;
using Pinoles.Api.Domain.Entities;

namespace Pinoles.Api.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(PinolesDbContext db, ILogger logger)
    {
        if (await db.Users.AnyAsync()) return;

        var users = new[]
        {
            new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Name = "Admin User",
                Email = "admin@pinoles.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!", 12),
                Role = "ADMIN",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "manager",
                Name = "Manager User",
                Email = "manager@pinoles.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager123!", 12),
                Role = "MANAGER",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "accounting",
                Name = "Accounting User",
                Email = "accounting@pinoles.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Accounting123!", 12),
                Role = "ACCOUNTING",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "warehouse",
                Name = "Warehouse User",
                Email = "warehouse@pinoles.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Warehouse123!", 12),
                Role = "WAREHOUSE",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };

        db.Users.AddRange(users);
        await db.SaveChangesAsync();
        logger.LogInformation("Seeded {Count} test users", users.Length);
    }
}
