using Pinoles.Api.Application.Common;
using Xunit;

namespace Pinoles.Api.Tests.Application.Common;

public class BcListQueryTests
{
    private static readonly string[] Allowed = { "displayName", "number" };

    [Fact]
    public void Build_InvalidPage_ClampsToOne()
    {
        var options = BcListQuery.Build(0, 20, null, null, Allowed);
        Assert.Equal(0, options.Skip); // (1 - 1) * 20
        Assert.Equal(20, options.Top);
    }

    [Fact]
    public void Build_InvalidPageSize_DefaultsToDefaultPageSize()
    {
        var tooBig = BcListQuery.Build(1, 9999, null, null, Allowed);
        Assert.Equal(BcListQuery.DefaultPageSize, tooBig.Top);

        var tooSmall = BcListQuery.Build(1, 0, null, null, Allowed);
        Assert.Equal(BcListQuery.DefaultPageSize, tooSmall.Top);
    }

    [Fact]
    public void Build_SortNotInAllowList_FallsBackToDefault()
    {
        var options = BcListQuery.Build(1, 20, "bogusField", "desc", Allowed);
        // Default sort field is the first allow-list entry; direction is preserved.
        Assert.Equal("displayName desc", options.OrderBy);
    }

    [Fact]
    public void Build_ValidSort_UsesRequestedField()
    {
        var options = BcListQuery.Build(1, 20, "number", "asc", Allowed);
        Assert.Equal("number asc", options.OrderBy);
    }

    [Fact]
    public void Build_SearchPresent_BuildsFilter()
    {
        var options = BcListQuery.Build(
            1, 20, null, null, Allowed,
            "Acme",
            term => $"contains(displayName,'{term}')");
        Assert.Equal("contains(displayName,'Acme')", options.Filter);
    }

    [Fact]
    public void Build_SearchEmpty_NoFilter()
    {
        var options = BcListQuery.Build(
            1, 20, null, null, Allowed,
            "   ",
            term => $"contains(displayName,'{term}')");
        Assert.Null(options.Filter);
    }

    [Fact]
    public void Build_SearchWithQuote_IsEscaped()
    {
        var options = BcListQuery.Build(
            1, 20, null, null, Allowed,
            "O'Brien",
            term => $"contains(displayName,'{term}')");
        Assert.Equal("contains(displayName,'O''Brien')", options.Filter);
    }

    [Fact]
    public void Build_PageTwo_ComputesSkip()
    {
        var options = BcListQuery.Build(2, 20, null, null, Allowed);
        Assert.Equal(20, options.Skip); // (2 - 1) * 20
    }
}
