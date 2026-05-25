using Pinoles.Api.Application.Common;
using Xunit;

namespace Pinoles.Api.Tests.Application.Common;

public class InvoiceTotalsTests
{
    private readonly record struct Line(decimal Amount, decimal Vat);

    [Fact]
    public void Compute_SumsSubtotalAndVat()
    {
        var lines = new[]
        {
            new Line(2000m, 20m),
            new Line(3000m, 20m),
        };

        var result = InvoiceTotals.Compute(lines, l => l.Amount, l => l.Vat);

        Assert.Equal(5000m, result.Subtotal);
        Assert.Equal(1000m, result.VatAmount);
        Assert.Equal(6000m, result.Total);
    }

    [Fact]
    public void Compute_MixedVatRates_AppliesPerLine()
    {
        var lines = new[]
        {
            new Line(1000m, 20m),
            new Line(1000m, 10m),
        };

        var result = InvoiceTotals.Compute(lines, l => l.Amount, l => l.Vat);

        Assert.Equal(2000m, result.Subtotal);
        Assert.Equal(300m, result.VatAmount);
        Assert.Equal(2300m, result.Total);
    }

    [Fact]
    public void Compute_NoLines_ReturnsZeros()
    {
        var result = InvoiceTotals.Compute(Array.Empty<Line>(), l => l.Amount, l => l.Vat);

        Assert.Equal(0m, result.Subtotal);
        Assert.Equal(0m, result.VatAmount);
        Assert.Equal(0m, result.Total);
    }

    [Fact]
    public void Compute_TotalAlwaysEqualsSubtotalPlusVat()
    {
        var lines = new[]
        {
            new Line(1234.50m, 20m),
            new Line(987.00m, 10m),
        };

        var result = InvoiceTotals.Compute(lines, l => l.Amount, l => l.Vat);

        Assert.Equal(result.Subtotal + result.VatAmount, result.Total);
    }
}
