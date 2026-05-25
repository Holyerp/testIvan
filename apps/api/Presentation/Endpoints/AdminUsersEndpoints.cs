using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Pinoles.Api.Application.Admin;
using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

/// <summary>
/// Admin user-management endpoints (US-021). All routes require the ADMIN role
/// (<c>RequireAdmin</c> policy). Writes target the LOCAL users table; every mutation is
/// audited. Canonical envelope: <c>{ success, data }</c> / <c>{ success, error, code }</c>.
/// </summary>
public static class AdminUsersEndpoints
{
    public static void MapAdminUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/admin/users")
            .WithTags("AdminUsers")
            .RequireAuthorization("RequireAdmin");

        group.MapGet("/", GetUsers);
        group.MapPost("/", CreateUser);
        group.MapPut("/{id}", UpdateUser);
        group.MapPost("/{id}/reset-password", ResetPassword);
        group.MapDelete("/{id}", DeleteUser);
    }

    internal static async Task<IResult> GetUsers(
        IUserAdminService service,
        CancellationToken ct)
    {
        try
        {
            var users = await service.GetUsersAsync(ct);
            return Results.Ok(new { success = true, data = users });
        }
        catch
        {
            return InternalError();
        }
    }

    internal static async Task<IResult> CreateUser(
        [FromBody] CreateUserRequest request,
        IUserAdminService service,
        HttpContext ctx,
        CancellationToken ct)
    {
        if (!TryGetActor(ctx, out var actorId, out var actorUsername))
            return Unauthorized();

        try
        {
            var result = await service.CreateUserAsync(request, actorId, actorUsername, ct);
            return MapResult(result, successStatus: 201);
        }
        catch
        {
            return InternalError();
        }
    }

    internal static async Task<IResult> UpdateUser(
        string id,
        [FromBody] UpdateUserRequest request,
        IUserAdminService service,
        HttpContext ctx,
        CancellationToken ct)
    {
        if (!TryGetActor(ctx, out var actorId, out var actorUsername))
            return Unauthorized();
        if (!Guid.TryParse(id, out var userId))
            return NotFoundUser();

        try
        {
            var result = await service.UpdateUserAsync(userId, request, actorId, actorUsername, ct);
            return MapResult(result);
        }
        catch
        {
            return InternalError();
        }
    }

    internal static async Task<IResult> ResetPassword(
        string id,
        IUserAdminService service,
        HttpContext ctx,
        CancellationToken ct)
    {
        if (!TryGetActor(ctx, out var actorId, out var actorUsername))
            return Unauthorized();
        if (!Guid.TryParse(id, out var userId))
            return NotFoundUser();

        try
        {
            var result = await service.ResetPasswordAsync(userId, actorId, actorUsername, ct);
            return MapResult(result);
        }
        catch
        {
            return InternalError();
        }
    }

    internal static async Task<IResult> DeleteUser(
        string id,
        IUserAdminService service,
        HttpContext ctx,
        CancellationToken ct)
    {
        if (!TryGetActor(ctx, out var actorId, out var actorUsername))
            return Unauthorized();
        if (!Guid.TryParse(id, out var userId))
            return NotFoundUser();

        try
        {
            var result = await service.DeleteUserAsync(userId, actorId, actorUsername, ct);
            return MapResult(result);
        }
        catch
        {
            return InternalError();
        }
    }

    /// <summary>Map a service result to the canonical envelope + HTTP status by error code.</summary>
    private static IResult MapResult(UserAdminResult result, int successStatus = 200)
    {
        if (result.Success)
            return Results.Json(new { success = true, data = result.User }, statusCode: successStatus);

        var code = result.ErrorCode ?? "INTERNAL_ERROR";
        var status = code switch
        {
            "NOT_FOUND_USER" => 404,
            "CONFLICT_USERNAME_TAKEN" => 409,
            "CONFLICT_EMAIL_TAKEN" => 409,
            "CONFLICT_LAST_ADMIN" => 409,
            "CONFLICT_CANNOT_DELETE_SELF" => 409,
            var c when c.StartsWith("VALIDATION_") => 400,
            _ => 500,
        };
        return Results.Json(new { success = false, error = code, code }, statusCode: status);
    }

    private static bool TryGetActor(HttpContext ctx, out Guid actorId, out string actorUsername)
    {
        actorId = Guid.Empty;
        actorUsername = string.Empty;

        var idClaim = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var nameClaim = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out actorId))
            return false;

        actorUsername = nameClaim ?? string.Empty;
        return true;
    }

    private static IResult Unauthorized() =>
        Results.Json(new { success = false, error = "Unauthorized", code = "AUTH_REQUIRED" }, statusCode: 401);

    private static IResult NotFoundUser() =>
        Results.Json(new { success = false, error = "User not found", code = "NOT_FOUND_USER" }, statusCode: 404);

    private static IResult InternalError() =>
        Results.Json(new { success = false, error = "Internal server error", code = "INTERNAL_ERROR" }, statusCode: 500);
}
