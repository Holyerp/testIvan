using System.Security.Claims;
using Pinoles.Api.Domain.Constants;

namespace Pinoles.Api.Presentation.Endpoints;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/users").WithTags("Users").RequireAuthorization();

        group.MapGet("/me", GetMe);
    }

    internal static IResult GetMe(HttpContext ctx)
    {
        var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = ctx.User.FindFirst(ClaimTypes.Name)?.Value;
        var role = ctx.User.FindFirst("role")?.Value
            ?? ctx.User.FindFirst(ClaimTypes.Role)?.Value;

        if (userId == null)
            return Results.Json(
                new { success = false, error = "Unauthorized", code = "AUTH_REQUIRED" },
                statusCode: 401);

        return Results.Ok(new
        {
            success = true,
            data = new { id = userId, username, role }
        });
    }
}
