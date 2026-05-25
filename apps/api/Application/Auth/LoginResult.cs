namespace Pinoles.Api.Application.Auth;

public record LoginResult(
    bool Success,
    string? AccessToken,
    DateTime? AccessTokenExpiry,
    string? NewRefreshToken,
    string? UserId,
    string? Username,
    string? Role,
    string? ErrorCode = null
);
