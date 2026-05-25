using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

public static class CustomersEndpoints
{
    public static void MapCustomersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/customers")
            .WithTags("Customers")
            .RequireAuthorization("RequireFinancial");

        group.MapGet("/", GetCustomers);
    }

    internal static async Task<IResult> GetCustomers(
        ICustomerService customers,
        int? page,
        int? pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await customers.GetCustomersAsync(
                page ?? 1, pageSize ?? 20, search, sortBy, sortDir, cancellationToken);
            return Results.Ok(new { success = true, data = result });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch customers", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
