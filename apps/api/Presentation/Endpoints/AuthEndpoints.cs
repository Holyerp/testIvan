using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Infrastructure.Auth;

namespace Pinoles.Api.Presentation.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/auth").WithTags("Auth");

        group.MapPost("/login", Login).AllowAnonymous();
        group.MapPost("/refresh", Refresh).AllowAnonymous();
        group.MapPost("/logout", Logout).RequireAuthorization();
    }

    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        IAuthService auth,
        LoginRateLimiter rateLimiter,
        HttpContext ctx,
        IOptions<JwtOptions> jwtOptions)
    {
        var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        if (rateLimiter.IsRateLimited(ip))
            return Results.Json(
                new { success = false, error = "Too many attempts", code = "AUTH_RATE_LIMITED" },
                statusCode: 429);

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Results.Json(
                new { success = false, error = "Username and password required", code = "VALIDATION_REQUIRED_FIELDS" },
                statusCode: 400);

        var result = await auth.LoginAsync(request.Username, request.Password);

        if (!result.Success)
        {
            rateLimiter.RecordFailure(ip);
            return Results.Json(
                new { success = false, error = "Invalid credentials", code = result.ErrorCode },
                statusCode: 401);
        }

        rateLimiter.Reset(ip);

        ctx.Response.Cookies.Append("refresh_token", result.NewRefreshToken!, new CookieOptions
        {
            HttpOnly = true,
            Secure = ctx.Request.IsHttps,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpiryDays),
            Path = "/api/v1/auth",
        });

        return Results.Ok(new
        {
            success = true,
            data = new
            {
                accessToken = result.AccessToken,
                expiresAt = result.AccessTokenExpiry,
                user = new { id = result.UserId, username = result.Username, role = result.Role }
            }
        });
    }

    private static async Task<IResult> Refresh(
        IAuthService auth,
        HttpContext ctx,
        IOptions<JwtOptions> jwtOptions)
    {
        var refreshToken = ctx.Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
            return Results.Json(
                new { success = false, error = "Refresh token missing", code = "AUTH_REFRESH_TOKEN_MISSING" },
                statusCode: 401);

        var result = await auth.RefreshAsync(refreshToken);
        if (!result.Success)
            return Results.Json(
                new { success = false, error = "Invalid or expired refresh token", code = result.ErrorCode },
                statusCode: 401);

        ctx.Response.Cookies.Append("refresh_token", result.NewRefreshToken!, new CookieOptions
        {
            HttpOnly = true,
            Secure = ctx.Request.IsHttps,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpiryDays),
            Path = "/api/v1/auth",
        });

        return Results.Ok(new
        {
            success = true,
            data = new
            {
                accessToken = result.AccessToken,
                expiresAt = result.AccessTokenExpiry,
                user = new { id = result.UserId, username = result.Username, role = result.Role }
            }
        });
    }

    private static async Task<IResult> Logout(
        IAuthService auth,
        HttpContext ctx)
    {
        var refreshToken = ctx.Request.Cookies["refresh_token"];
        if (!string.IsNullOrEmpty(refreshToken))
            await auth.LogoutAsync(refreshToken);

        ctx.Response.Cookies.Delete("refresh_token", new CookieOptions { Path = "/api/v1/auth" });
        return Results.Ok(new { success = true, data = (object?)null });
    }
}

public record LoginRequest(string Username, string Password);
