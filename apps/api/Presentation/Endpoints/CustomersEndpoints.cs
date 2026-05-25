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
        group.MapGet("/{id}", GetCustomerById);
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

    internal static async Task<IResult> GetCustomerById(
        string id,
        ICustomerService customers,
        CancellationToken cancellationToken)
    {
        try
        {
            var detail = await customers.GetCustomerByIdAsync(id, cancellationToken);
            if (detail == null)
            {
                return Results.Json(
                    new { success = false, error = "Customer not found", code = "NOT_FOUND_CUSTOMER" },
                    statusCode: 404);
            }

            return Results.Ok(new { success = true, data = detail });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch customer", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
