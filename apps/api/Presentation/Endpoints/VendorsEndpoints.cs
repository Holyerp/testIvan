using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

public static class VendorsEndpoints
{
    public static void MapVendorsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/vendors")
            .WithTags("Vendors")
            .RequireAuthorization("RequireFinancial");

        group.MapGet("/", GetVendors);
    }

    internal static async Task<IResult> GetVendors(
        IVendorService vendors,
        int? page,
        int? pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await vendors.GetVendorsAsync(
                page ?? 1, pageSize ?? 20, search, sortBy, sortDir, cancellationToken);
            return Results.Ok(new { success = true, data = result });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch vendors", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
