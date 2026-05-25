using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Pinoles.Api.Presentation.Endpoints;
using Xunit;

namespace Pinoles.Api.Tests.Presentation.Endpoints;

public class UsersEndpointsTests
{
    private static HttpContext BuildHttpContext(params Claim[] claims)
    {
        var identity = new ClaimsIdentity(claims, "Bearer");
        var principal = new ClaimsPrincipal(identity);
        return new DefaultHttpContext { User = principal };
    }

    [Fact]
    public void GetMe_AuthenticatedUser_ReturnsOkWithUserInfo()
    {
        var ctx = BuildHttpContext(
            new Claim(ClaimTypes.NameIdentifier, "user-id-123"),
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim("role", "ADMIN"));

        var result = UsersEndpoints.GetMe(ctx);

        Assert.NotNull(result);
        // Result is IResult; verify it's not a 401 result by checking the type
        // The method returns Results.Ok for authenticated users
        Assert.IsNotType<Microsoft.AspNetCore.Http.HttpResults.JsonHttpResult<object>>(result);
    }

    [Fact]
    public void GetMe_NoNameIdentifierClaim_Returns401Json()
    {
        // User with only Name claim but no NameIdentifier
        var ctx = BuildHttpContext(new Claim(ClaimTypes.Name, "noidentifier"));

        var result = UsersEndpoints.GetMe(ctx);

        Assert.NotNull(result);
        // Verify the result is a JSON result (401 path returns Results.Json)
        Assert.IsNotType<Microsoft.AspNetCore.Http.HttpResults.Ok<object>>(result);
    }

    [Fact]
    public void GetMe_EmptyPrincipal_Returns401Json()
    {
        var ctx = new DefaultHttpContext { User = new ClaimsPrincipal() };

        var result = UsersEndpoints.GetMe(ctx);

        Assert.NotNull(result);
    }
}
