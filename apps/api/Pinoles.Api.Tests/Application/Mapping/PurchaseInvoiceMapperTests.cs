using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Mapping;
using Xunit;

namespace Pinoles.Api.Tests.Application.Mapping;

public class PurchaseInvoiceMapperTests
{
    private readonly PurchaseInvoiceMapper _mapper = new();

    [Fact]
    public void Map_CopiesAllFields_AndNormalizesStatus()
    {
        var source = new BcPurchaseInvoice
        {
            Id = "pinv001",
            Number = "PI-001",
            VendorName = "Supplier A d.o.o.",
            PostingDate = "2026-01-15",
            DueDate = "2026-02-14",
            TotalAmountIncludingTax = 38000.00m,
            Status = "Open",
        };

        var dto = _mapper.Map(source);

        Assert.Equal("pinv001", dto.Id);
        Assert.Equal("PI-001", dto.Number);
        Assert.Equal("Supplier A d.o.o.", dto.VendorName);
        Assert.Equal("2026-01-15", dto.PostingDate);
        Assert.Equal("2026-02-14", dto.DueDate);
        Assert.Equal(38000.00m, dto.Amount);
        Assert.Equal("OPEN", dto.Status);
    }

    [Theory]
    [InlineData("Open", "OPEN")]
    [InlineData("Paid", "PAID")]
    [InlineData("Partially Paid", "PARTIAL")]
    [InlineData("partial", "PARTIAL")]
    [InlineData("Something Unknown", "OPEN")]
    [InlineData("", "OPEN")]
    public void Map_NormalizesStatusToWireValue(string bcStatus, string expected)
    {
        var source = new BcPurchaseInvoice { Id = "pinv001", Number = "PI-001", Status = bcStatus };
        var dto = _mapper.Map(source);
        Assert.Equal(expected, dto.Status);
    }

    [Fact]
    public void Map_PreservesVendorName()
    {
        var source = new BcPurchaseInvoice { Id = "pinv003", Number = "PI-003", VendorName = "Materijal Promet", Status = "Partially Paid" };
        var dto = _mapper.Map(source);
        Assert.Equal("Materijal Promet", dto.VendorName);
        Assert.Equal("PARTIAL", dto.Status);
    }

    [Fact]
    public void Map_ProducesNewInstanceEachCall()
    {
        var source = new BcPurchaseInvoice { Id = "pinv008", Number = "PI-008", Status = "Paid" };
        var first = _mapper.Map(source);
        var second = _mapper.Map(source);
        Assert.NotSame(first, second);
        Assert.Equal("PAID", second.Status);
    }
}
