using Pinoles.Api.Domain.Constants;
using Xunit;

namespace Pinoles.Api.Tests.Domain;

public class UserRolesTests
{
    [Theory]
    [InlineData("ADMIN", true)]
    [InlineData("MANAGER", true)]
    [InlineData("ACCOUNTING", true)]
    [InlineData("WAREHOUSE", true)]
    [InlineData("SUPERUSER", false)]
    [InlineData("", false)]
    [InlineData("admin", false)]
    public void IsValid_ReturnsExpectedResult(string role, bool expected)
    {
        Assert.Equal(expected, UserRoles.IsValid(role));
    }

    [Fact]
    public void All_ContainsFourRoles()
    {
        Assert.Equal(4, UserRoles.All.Length);
    }

    [Fact]
    public void Constants_HaveCorrectValues()
    {
        Assert.Equal("ADMIN", UserRoles.Admin);
        Assert.Equal("MANAGER", UserRoles.Manager);
        Assert.Equal("ACCOUNTING", UserRoles.Accounting);
        Assert.Equal("WAREHOUSE", UserRoles.Warehouse);
    }
}
