using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Domain.Entities;
using Pinoles.Api.Infrastructure.Auth;
using Pinoles.Api.Infrastructure.Persistence;

namespace Pinoles.Api.Application.Auth;

public class AuthService : IAuthService
{
    private readonly PinolesDbContext _db;
    private readonly ITokenService _tokens;
    private readonly JwtOptions _jwt;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        PinolesDbContext db,
        ITokenService tokens,
        IOptions<JwtOptions> jwt,
        ILogger<AuthService> logger)
    {
        _db = db;
        _tokens = tokens;
        _jwt = jwt.Value;
        _logger = logger;
    }

    public async Task<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(
            u => u.Username == username && u.IsActive, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            _logger.LogWarning("Failed login attempt for username {Username}", username);
            return new LoginResult(false, null, null, null, null, null, null, "AUTH_INVALID_CREDENTIALS");
        }

        var accessToken = _tokens.GenerateAccessToken(user);
        var refreshToken = _tokens.GenerateRefreshToken();

        var rt = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpiryDays),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
        };
        _db.RefreshTokens.Add(rt);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("auth.login.success userId={UserId} role={Role}", user.Id, user.Role);
        return new LoginResult(
            true,
            accessToken,
            DateTime.UtcNow.AddHours(_jwt.AccessTokenExpiryHours),
            refreshToken,
            user.Id.ToString(),
            user.Username,
            user.Role);
    }

    public async Task<LoginResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var rt = await _db.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(
                r => r.Token == refreshToken && !r.IsRevoked && r.ExpiresAt > DateTime.UtcNow,
                cancellationToken);

        if (rt == null)
            return new LoginResult(false, null, null, null, null, null, null, "AUTH_INVALID_REFRESH_TOKEN");

        rt.IsRevoked = true;

        var newRefreshToken = _tokens.GenerateRefreshToken();
        var newRt = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = newRefreshToken,
            UserId = rt.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpiryDays),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
        };
        _db.RefreshTokens.Add(newRt);
        await _db.SaveChangesAsync(cancellationToken);

        var accessToken = _tokens.GenerateAccessToken(rt.User);
        return new LoginResult(
            true,
            accessToken,
            DateTime.UtcNow.AddHours(_jwt.AccessTokenExpiryHours),
            newRefreshToken,
            rt.User.Id.ToString(),
            rt.User.Username,
            rt.User.Role);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var rt = await _db.RefreshTokens.FirstOrDefaultAsync(
            r => r.Token == refreshToken, cancellationToken);
        if (rt != null)
        {
            rt.IsRevoked = true;
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
