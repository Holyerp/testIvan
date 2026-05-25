using Pinoles.Api.Domain.Constants;
using Xunit;

namespace Pinoles.Api.Tests.Domain;

public class ItemLedgerEntryTypeTests
{
    [Theory]
    [InlineData("PURCHASE", true)]
    [InlineData("SALE", true)]
    [InlineData("ADJUSTMENT", true)]
    [InlineData("TRANSFER", true)]
    [InlineData("INVOICE", false)]
    [InlineData("purchase", false)] // case-sensitive wire value
    [InlineData("", false)]
    public void IsValid_ReturnsExpectedResult(string type, bool expected)
    {
        Assert.Equal(expected, ItemLedgerEntryType.IsValid(type));
    }

    [Fact]
    public void All_ContainsFourTypes()
    {
        Assert.Equal(4, ItemLedgerEntryType.All.Length);
    }

    [Fact]
    public void Constants_HaveCorrectWireValues()
    {
        Assert.Equal("PURCHASE", ItemLedgerEntryType.Purchase);
        Assert.Equal("SALE", ItemLedgerEntryType.Sale);
        Assert.Equal("ADJUSTMENT", ItemLedgerEntryType.Adjustment);
        Assert.Equal("TRANSFER", ItemLedgerEntryType.Transfer);
    }

    [Theory]
    [InlineData("Purchase", "PURCHASE")]
    [InlineData("sale", "SALE")]
    [InlineData("ADJUSTMENT", "ADJUSTMENT")]
    [InlineData("Transfer", "TRANSFER")]
    [InlineData("Unknown", "ADJUSTMENT")] // fallback to a documented member
    [InlineData("", "ADJUSTMENT")]
    [InlineData(null, "ADJUSTMENT")]
    public void Normalize_CoercesToWireValue(string? raw, string expected)
    {
        Assert.Equal(expected, ItemLedgerEntryType.Normalize(raw));
    }
}
