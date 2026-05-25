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
        group.MapGet("/{id}", GetVendorById);
        group.MapGet("/{id}/invoices", GetVendorInvoices);
    }

    private const string NotFoundVendor = "NOT_FOUND_VENDOR";

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

    internal static async Task<IResult> GetVendorById(
        string id,
        IVendorService vendors,
        CancellationToken cancellationToken)
    {
        try
        {
            var detail = await vendors.GetVendorByIdAsync(id, cancellationToken);
            if (detail == null)
            {
                return Results.Json(
                    new { success = false, error = "Vendor not found", code = NotFoundVendor },
                    statusCode: 404);
            }

            return Results.Ok(new { success = true, data = detail });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch vendor", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    internal static async Task<IResult> GetVendorInvoices(
        string id,
        IVendorService vendors,
        CancellationToken cancellationToken)
    {
        try
        {
            var invoices = await vendors.GetVendorInvoicesForEndpointAsync(id, cancellationToken);
            if (invoices == null)
            {
                return Results.Json(
                    new { success = false, error = "Vendor not found", code = NotFoundVendor },
                    statusCode: 404);
            }

            return Results.Ok(new { success = true, data = invoices });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch vendor invoices", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
