using System.Security.Claims;
using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this WebApplication app)
    {
        // Search itself is available to ALL authenticated roles (incl. WAREHOUSE);
        // per-group data access is enforced inside SearchService from the caller's roles.
        var group = app.MapGroup("/api/v1/search")
            .WithTags("Search")
            .RequireAuthorization("RequireDashboard");

        group.MapGet("/", Search);
    }

    internal static async Task<IResult> Search(
        ISearchService search,
        HttpContext ctx,
        string? q,
        int? limit,
        CancellationToken cancellationToken)
    {
        try
        {
            var roles = ReadRoles(ctx.User);
            var result = await search.SearchAsync(q ?? string.Empty, limit ?? 5, roles, cancellationToken);
            return Results.Ok(new { success = true, data = result });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Search is unavailable", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    // Reads the caller's role(s) the same way UsersEndpoints.GetMe does: the custom
    // "role" claim first, falling back to the standard ClaimTypes.Role.
    private static IEnumerable<string> ReadRoles(ClaimsPrincipal user)
    {
        var roles = user.FindAll("role").Select(c => c.Value)
            .Concat(user.FindAll(ClaimTypes.Role).Select(c => c.Value))
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .Distinct()
            .ToList();

        return roles;
    }
}
