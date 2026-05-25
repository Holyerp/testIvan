using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Mapping;
using Xunit;

namespace Pinoles.Api.Tests.Application.Mapping;

public class SalesInvoiceMapperTests
{
    private readonly SalesInvoiceMapper _mapper = new();

    [Theory]
    [InlineData("Open", "OPEN")]
    [InlineData("Paid", "PAID")]
    [InlineData("Partially Paid", "PARTIAL")]
    [InlineData("partial", "PARTIAL")]
    [InlineData("Something Unknown", "OPEN")]
    [InlineData("", "OPEN")]
    public void NormalizeStatus_MapsBcStatusToWireValue(string bcStatus, string expected)
    {
        Assert.Equal(expected, SalesInvoiceMapper.NormalizeStatus(bcStatus));
    }

    [Fact]
    public void Map_CopiesAllFields_AndNormalizesStatus()
    {
        var source = new BcSalesInvoice
        {
            Id = "inv001",
            Number = "SI-001",
            CustomerName = "Acme d.o.o.",
            PostingDate = "2026-01-15",
            DueDate = "2026-02-14",
            TotalAmountIncludingTax = 45000.00m,
            Status = "Open",
        };

        var dto = _mapper.Map(source);

        Assert.Equal("inv001", dto.Id);
        Assert.Equal("SI-001", dto.Number);
        Assert.Equal("Acme d.o.o.", dto.CustomerName);
        Assert.Equal("2026-01-15", dto.PostingDate);
        Assert.Equal("2026-02-14", dto.DueDate);
        Assert.Equal(45000.00m, dto.Amount);
        Assert.Equal("OPEN", dto.Status);
    }

    [Fact]
    public void Map_NormalizesPartiallyPaid()
    {
        var source = new BcSalesInvoice { Id = "inv003", Number = "SI-003", Status = "Partially Paid" };
        var dto = _mapper.Map(source);
        Assert.Equal("PARTIAL", dto.Status);
    }

    [Fact]
    public void Map_ProducesNewInstanceEachCall()
    {
        var source = new BcSalesInvoice { Id = "inv008", Number = "SI-008", Status = "Paid" };
        var first = _mapper.Map(source);
        var second = _mapper.Map(source);
        Assert.NotSame(first, second);
        Assert.Equal("PAID", second.Status);
    }
}
