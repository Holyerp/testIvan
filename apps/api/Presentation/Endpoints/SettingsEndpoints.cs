using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

/// <summary>
/// Admin Settings endpoints (US-022). All routes require the ADMIN role (<c>RequireAdmin</c>
/// policy). Read-only: surfaces the NON-SENSITIVE Business Central connection config and an
/// on-demand connectivity probe. The BC service-principal credentials are NEVER returned.
/// Canonical envelope: <c>{ success, data }</c> / <c>{ success, error, code }</c>.
/// </summary>
public static class SettingsEndpoints
{
    public static void MapSettingsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/settings")
            .WithTags("Settings")
            .RequireAuthorization("RequireAdmin");

        group.MapGet("/bc-config", GetBcConfig);
        group.MapPost("/bc-config/test", TestConnection);
    }

    internal static async Task<IResult> GetBcConfig(
        ISettingsService settings,
        CancellationToken cancellationToken)
    {
        try
        {
            var config = await settings.GetBcConfigAsync(cancellationToken);
            return Results.Ok(new { success = true, data = config });
        }
        catch
        {
            return InternalError();
        }
    }

    // The probe itself never 502s — connectivity success/failure is carried inside the result
    // payload (data.success). A 500 here means the probe machinery itself faulted, not BC.
    internal static async Task<IResult> TestConnection(
        ISettingsService settings,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await settings.TestConnectionAsync(cancellationToken);
            return Results.Ok(new { success = true, data = result });
        }
        catch
        {
            return InternalError();
        }
    }

    private static IResult InternalError() =>
        Results.Json(new { success = false, error = "Internal server error", code = "INTERNAL_ERROR" }, statusCode: 500);
}
