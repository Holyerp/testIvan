using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/dashboard")
            .WithTags("Dashboard")
            .RequireAuthorization("RequireDashboard");

        group.MapGet("/kpis", GetKpis);
    }

    internal static async Task<IResult> GetKpis(
        IDashboardService dashboard,
        CancellationToken cancellationToken)
    {
        try
        {
            var kpis = await dashboard.GetKpisAsync(cancellationToken);
            return Results.Ok(new { success = true, data = kpis });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch dashboard data", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
