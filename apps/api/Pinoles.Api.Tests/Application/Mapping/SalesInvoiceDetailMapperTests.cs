using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Mapping;
using Xunit;

namespace Pinoles.Api.Tests.Application.Mapping;

public class SalesInvoiceDetailMapperTests
{
    private readonly SalesInvoiceDetailMapper _mapper = new();

    private static BcSalesInvoice SampleInvoice(string status = "Open") => new()
    {
        Id = "inv001",
        Number = "SI-001",
        CustomerName = "Acme d.o.o.",
        BillToAddress = "Acme d.o.o., Bulevar 1, Beograd",
        PostingDate = "2026-01-15",
        DueDate = "2026-02-14",
        PaymentTerms = "30 dana",
        Status = status,
        SalesInvoiceLines = new List<BcSalesInvoiceLine>
        {
            new() { Description = "Usluga A", Quantity = 2m, UnitPrice = 1000m, VatPercent = 20m, LineAmount = 2000m },
            new() { Description = "Usluga B", Quantity = 1m, UnitPrice = 3000m, VatPercent = 20m, LineAmount = 3000m },
        },
    };

    [Fact]
    public void Map_CopiesHeaderFields()
    {
        var dto = _mapper.Map(SampleInvoice());

        Assert.Equal("inv001", dto.Header.Id);
        Assert.Equal("SI-001", dto.Header.Number);
        Assert.Equal("Acme d.o.o.", dto.Header.CustomerName);
        Assert.Equal("Acme d.o.o., Bulevar 1, Beograd", dto.Header.BillToAddress);
        Assert.Equal("2026-01-15", dto.Header.PostingDate);
        Assert.Equal("2026-02-14", dto.Header.DueDate);
        Assert.Equal("30 dana", dto.Header.PaymentTerms);
    }

    [Fact]
    public void Map_NormalizesStatus()
    {
        Assert.Equal("OPEN", _mapper.Map(SampleInvoice("Open")).Header.Status);
        Assert.Equal("PAID", _mapper.Map(SampleInvoice("Paid")).Header.Status);
        Assert.Equal("PARTIAL", _mapper.Map(SampleInvoice("Partially Paid")).Header.Status);
        Assert.Equal("OPEN", _mapper.Map(SampleInvoice("Whatever")).Header.Status);
    }

    [Fact]
    public void Map_MapsLines()
    {
        var dto = _mapper.Map(SampleInvoice());

        Assert.Equal(2, dto.Lines.Count);
        var first = dto.Lines[0];
        Assert.Equal("Usluga A", first.Description);
        Assert.Equal(2m, first.Quantity);
        Assert.Equal(1000m, first.UnitPrice);
        Assert.Equal(20m, first.VatPercent);
        Assert.Equal(2000m, first.LineTotal);
    }

    [Fact]
    public void Map_ComputesTotals()
    {
        var dto = _mapper.Map(SampleInvoice());

        // subtotal = 2000 + 3000 = 5000; vat = 20% = 1000; total = 6000
        Assert.Equal(5000m, dto.Totals.Subtotal);
        Assert.Equal(1000m, dto.Totals.VatAmount);
        Assert.Equal(6000m, dto.Totals.Total);
        Assert.Equal(dto.Totals.Subtotal + dto.Totals.VatAmount, dto.Totals.Total);
    }

    [Fact]
    public void Map_NoLines_ProducesZeroTotals()
    {
        var source = SampleInvoice();
        source.SalesInvoiceLines.Clear();

        var dto = _mapper.Map(source);

        Assert.Empty(dto.Lines);
        Assert.Equal(0m, dto.Totals.Subtotal);
        Assert.Equal(0m, dto.Totals.VatAmount);
        Assert.Equal(0m, dto.Totals.Total);
    }

    [Fact]
    public void Map_MixedVatRates_ComputesVatPerLine()
    {
        var source = SampleInvoice();
        source.SalesInvoiceLines = new List<BcSalesInvoiceLine>
        {
            new() { Description = "Standard VAT", Quantity = 1m, UnitPrice = 1000m, VatPercent = 20m, LineAmount = 1000m },
            new() { Description = "Reduced VAT", Quantity = 1m, UnitPrice = 1000m, VatPercent = 10m, LineAmount = 1000m },
        };

        var dto = _mapper.Map(source);

        // subtotal = 2000; vat = 200 + 100 = 300; total = 2300
        Assert.Equal(2000m, dto.Totals.Subtotal);
        Assert.Equal(300m, dto.Totals.VatAmount);
        Assert.Equal(2300m, dto.Totals.Total);
    }
}
