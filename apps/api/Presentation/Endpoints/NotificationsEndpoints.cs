using System.Security.Claims;
using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

public static class NotificationsEndpoints
{
    public static void MapNotificationsEndpoints(this WebApplication app)
    {
        // Available to ALL authenticated roles (incl. WAREHOUSE); the notification list is
        // role-filtered inside NotificationService (financial vs warehouse alerts).
        var group = app.MapGroup("/api/v1/notifications")
            .WithTags("Notifications")
            .RequireAuthorization("RequireDashboard");

        group.MapGet("/", GetNotifications);
    }

    internal static async Task<IResult> GetNotifications(
        INotificationService notifications,
        HttpContext ctx,
        CancellationToken cancellationToken)
    {
        try
        {
            var roles = ReadRoles(ctx.User);
            var data = await notifications.GetNotificationsAsync(roles, cancellationToken);
            return Results.Ok(new { success = true, data });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Notifications are unavailable", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    // Reads the caller's role(s) the same way SearchEndpoints / UsersEndpoints.GetMe does:
    // the custom "role" claim first, falling back to the standard ClaimTypes.Role.
    private static IEnumerable<string> ReadRoles(ClaimsPrincipal user)
    {
        return user.FindAll("role").Select(c => c.Value)
            .Concat(user.FindAll(ClaimTypes.Role).Select(c => c.Value))
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .Distinct()
            .ToList();
    }
}
