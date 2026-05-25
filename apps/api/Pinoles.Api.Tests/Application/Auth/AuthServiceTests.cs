using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.Audit;
using Pinoles.Api.Application.Auth;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Domain.Entities;
using Pinoles.Api.Infrastructure.Auth;
using Pinoles.Api.Infrastructure.Persistence;
using Xunit;

namespace Pinoles.Api.Tests.Application.Auth;

public class AuthServiceTests
{
    private static PinolesDbContext CreateDbContext()
    {
        var opts = new DbContextOptionsBuilder<PinolesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new PinolesDbContext(opts);
    }

    private static AuthService CreateService(PinolesDbContext db)
    {
        var jwtOpts = Options.Create(new JwtOptions
        {
            Secret = "test-secret-min-32-chars-long!!!!!",
            Issuer = "test",
            Audience = "test",
            AccessTokenExpiryHours = 8,
            RefreshTokenExpiryDays = 7,
        });
        var tokenService = new TokenService(jwtOpts);
        var logger = NullLogger<AuthService>.Instance;
        var audit = new AuditWriter(db);
        return new AuthService(db, tokenService, jwtOpts, audit, logger);
    }

    private static async Task<User> SeedUser(PinolesDbContext db, string username = "testuser", string role = "ADMIN")
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            // cost 4 for tests (fast)
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!", 4),
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsSuccess()
    {
        await using var db = CreateDbContext();
        await SeedUser(db);
        var service = CreateService(db);

        var result = await service.LoginAsync("testuser", "Password123!");

        Assert.True(result.Success);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.NewRefreshToken);
        Assert.Equal("ADMIN", result.Role);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_WritesAuditRowAndSetsLastLogin()
    {
        await using var db = CreateDbContext();
        var seeded = await SeedUser(db);
        var service = CreateService(db);

        await service.LoginAsync("testuser", "Password123!");

        var audit = await db.AuditLogs.SingleOrDefaultAsync(a => a.Action == AuditActions.AuthLoginSuccess);
        Assert.NotNull(audit);
        Assert.Equal(seeded.Id, audit!.UserId);
        Assert.Equal("testuser", audit.Username);

        var user = await db.Users.SingleAsync(u => u.Id == seeded.Id);
        Assert.NotNull(user.LastLoginAt);
    }

    [Fact]
    public async Task LoginAsync_FailedLogin_WritesNoAuditRow()
    {
        await using var db = CreateDbContext();
        await SeedUser(db);
        var service = CreateService(db);

        await service.LoginAsync("testuser", "WrongPassword!");

        Assert.False(await db.AuditLogs.AnyAsync());
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsFailure()
    {
        await using var db = CreateDbContext();
        await SeedUser(db);
        var service = CreateService(db);

        var result = await service.LoginAsync("testuser", "WrongPassword!");

        Assert.False(result.Success);
        Assert.Equal("AUTH_INVALID_CREDENTIALS", result.ErrorCode);
    }

    [Fact]
    public async Task LoginAsync_UnknownUser_ReturnsFailure()
    {
        await using var db = CreateDbContext();
        var service = CreateService(db);

        var result = await service.LoginAsync("nobody", "Password123!");

        Assert.False(result.Success);
        Assert.Equal("AUTH_INVALID_CREDENTIALS", result.ErrorCode);
    }

    [Fact]
    public async Task LoginAsync_InactiveUser_ReturnsFailure()
    {
        await using var db = CreateDbContext();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "inactive",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!", 4),
            Role = "ADMIN",
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var service = CreateService(db);

        var result = await service.LoginAsync("inactive", "Password123!");

        Assert.False(result.Success);
        Assert.Equal("AUTH_INVALID_CREDENTIALS", result.ErrorCode);
    }

    [Fact]
    public async Task RefreshAsync_ValidToken_RotatesToken()
    {
        await using var db = CreateDbContext();
        await SeedUser(db);
        var service = CreateService(db);

        var loginResult = await service.LoginAsync("testuser", "Password123!");
        var refreshResult = await service.RefreshAsync(loginResult.NewRefreshToken!);

        Assert.True(refreshResult.Success);
        Assert.NotNull(refreshResult.AccessToken);
        Assert.NotEqual(loginResult.NewRefreshToken, refreshResult.NewRefreshToken);
    }

    [Fact]
    public async Task RefreshAsync_InvalidToken_ReturnsFailure()
    {
        await using var db = CreateDbContext();
        var service = CreateService(db);

        var result = await service.RefreshAsync("invalid-token");

        Assert.False(result.Success);
        Assert.Equal("AUTH_INVALID_REFRESH_TOKEN", result.ErrorCode);
    }

    [Fact]
    public async Task RefreshAsync_RevokedToken_ReturnsFailure()
    {
        await using var db = CreateDbContext();
        await SeedUser(db);
        var service = CreateService(db);

        var loginResult = await service.LoginAsync("testuser", "Password123!");
        // First refresh — rotates token
        await service.RefreshAsync(loginResult.NewRefreshToken!);
        // Second refresh with old token — should fail
        var result = await service.RefreshAsync(loginResult.NewRefreshToken!);

        Assert.False(result.Success);
        Assert.Equal("AUTH_INVALID_REFRESH_TOKEN", result.ErrorCode);
    }

    [Fact]
    public async Task LogoutAsync_RevokesToken()
    {
        await using var db = CreateDbContext();
        await SeedUser(db);
        var service = CreateService(db);

        var loginResult = await service.LoginAsync("testuser", "Password123!");
        await service.LogoutAsync(loginResult.NewRefreshToken!);

        var refreshResult = await service.RefreshAsync(loginResult.NewRefreshToken!);
        Assert.False(refreshResult.Success);
    }
}
