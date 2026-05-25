using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Infrastructure.BusinessCentral;

public class BcQueryOptionsTests
{
    [Fact]
    public void ToQueryString_WithAllOptions_BuildsCorrectString()
    {
        var options = new BcQueryOptions
        {
            Filter  = "status eq 'Open'",
            Select  = "id,number,customerName",
            OrderBy = "postingDate desc",
            Top     = 10,
            Skip    = 20,
            Count   = true
        };

        var qs = options.ToQueryString();

        Assert.Contains("$top=10", qs);
        Assert.Contains("$skip=20", qs);
        Assert.Contains("$count=true", qs);
        Assert.Contains("$select=id,number,customerName", qs);
        Assert.Contains("$orderby=postingDate desc", qs);
        Assert.StartsWith("?", qs);
    }

    [Fact]
    public void ToQueryString_Empty_ReturnsEmptyString()
    {
        var options = new BcQueryOptions();

        Assert.Equal(string.Empty, options.ToQueryString());
    }

    [Fact]
    public void ToQueryString_FilterOnly_EncodesFilter()
    {
        var options = new BcQueryOptions { Filter = "name eq 'Test'" };

        var qs = options.ToQueryString();

        Assert.Contains("$filter=", qs);
        Assert.DoesNotContain("'Test'", qs); // should be URL-encoded
    }
}
