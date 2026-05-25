using Pinoles.Api.Application.Mapping;
using Xunit;

namespace Pinoles.Api.Tests.Application.Mapping;

// Tests for the shared status-normalization helper used by both the sales and
// purchase mappers (single source of truth — Rule of Three extraction in US-009).
public class InvoiceStatusTests
{
    [Theory]
    [InlineData("Open", "OPEN")]
    [InlineData("open", "OPEN")]
    [InlineData("Paid", "PAID")]
    [InlineData("Partially Paid", "PARTIAL")]
    [InlineData("partial", "PARTIAL")]
    [InlineData("Posted", "POSTED")]
    [InlineData("Something Unknown", "OPEN")]
    [InlineData("", "OPEN")]
    [InlineData(null, "OPEN")]
    public void Normalize_MapsBcStatusToWireValue(string? bcStatus, string expected)
    {
        Assert.Equal(expected, InvoiceStatus.Normalize(bcStatus));
    }

    [Theory]
    [InlineData("Open", "OPEN")]
    [InlineData("open", "OPEN")]
    [InlineData("Posted", "POSTED")]
    [InlineData("posted", "POSTED")]
    [InlineData("Paid", "OPEN")]
    [InlineData("Partially Paid", "OPEN")]
    [InlineData("", "OPEN")]
    [InlineData(null, "OPEN")]
    public void NormalizeCreditMemo_MapsBcStatusToWireValue(string? bcStatus, string expected)
    {
        Assert.Equal(expected, InvoiceStatus.NormalizeCreditMemo(bcStatus));
    }
}
