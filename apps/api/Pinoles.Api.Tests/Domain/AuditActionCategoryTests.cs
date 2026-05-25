using Pinoles.Api.Domain.Constants;
using Xunit;

namespace Pinoles.Api.Tests.Domain;

public class AuditActionCategoryTests
{
    [Theory]
    [InlineData("AUTH_LOGIN_SUCCESS", "LOGIN")]
    [InlineData("AUTH_LOGIN_FAILURE", "LOGIN")]
    [InlineData("ADMIN_USER_CREATED", "ADMIN")]
    [InlineData("ADMIN_USER_UPDATED", "ADMIN")]
    [InlineData("ADMIN_USER_DELETED", "ADMIN")]
    [InlineData("ADMIN_PASSWORD_RESET", "ADMIN")]
    [InlineData("AUDIT_LOG_EXPORT", "EXPORT")]
    [InlineData("CUSTOMER_VIEW", "VIEW")]
    [InlineData("SOMETHING_UNKNOWN", "ADMIN")] // default bucket
    [InlineData("", "ADMIN")]
    [InlineData(null, "ADMIN")]
    public void Categorize_MapsActionToCategory(string? action, string expected)
    {
        Assert.Equal(expected, AuditActionCategory.Categorize(action));
    }

    [Theory]
    [InlineData("LOGIN", true)]
    [InlineData("VIEW", true)]
    [InlineData("EXPORT", true)]
    [InlineData("ADMIN", true)]
    [InlineData("login", false)] // case-sensitive wire value
    [InlineData("OTHER", false)]
    [InlineData("", false)]
    public void IsValid_ReturnsExpectedResult(string category, bool expected)
    {
        Assert.Equal(expected, AuditActionCategory.IsValid(category));
    }

    [Fact]
    public void All_ContainsFourCategories()
    {
        Assert.Equal(4, AuditActionCategory.All.Length);
    }

    [Fact]
    public void Constants_HaveCorrectWireValues()
    {
        Assert.Equal("LOGIN", AuditActionCategory.Login);
        Assert.Equal("VIEW", AuditActionCategory.View);
        Assert.Equal("EXPORT", AuditActionCategory.Export);
        Assert.Equal("ADMIN", AuditActionCategory.Admin);
    }
}
