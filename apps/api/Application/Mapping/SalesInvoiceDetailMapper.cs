using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC sales invoice (with expanded line items) to the detail DTO the UI
/// consumes — header, line items, and computed totals. Reuses
/// <see cref="SalesInvoiceMapper.NormalizeStatus"/> for status normalization (DRY)
/// so the OPEN/PARTIAL/PAID wire value is identical to the list view, and
/// <see cref="InvoiceTotals"/> for the shared money math (also used by the purchase
/// detail mapper).
/// </summary>
public class SalesInvoiceDetailMapper : IBcMapper<BcSalesInvoice, SalesInvoiceDetailDto>
{
    public SalesInvoiceDetailDto Map(BcSalesInvoice source)
    {
        var lines = source.SalesInvoiceLines.Select(MapLine).ToList();

        var totals = InvoiceTotals.Compute(
            source.SalesInvoiceLines, l => l.LineAmount, l => l.VatPercent);

        return new SalesInvoiceDetailDto
        {
            Header = new SalesInvoiceHeaderDto
            {
                Id = source.Id,
                Number = source.Number,
                CustomerName = source.CustomerName,
                BillToAddress = source.BillToAddress,
                PostingDate = source.PostingDate,
                DueDate = source.DueDate,
                PaymentTerms = source.PaymentTerms,
                Status = SalesInvoiceMapper.NormalizeStatus(source.Status),
            },
            Lines = lines,
            Totals = new SalesInvoiceTotalsDto
            {
                Subtotal = totals.Subtotal,
                VatAmount = totals.VatAmount,
                Total = totals.Total,
            },
        };
    }

    private static SalesInvoiceLineDto MapLine(BcSalesInvoiceLine line) => new()
    {
        Description = line.Description,
        Quantity = line.Quantity,
        UnitPrice = line.UnitPrice,
        VatPercent = line.VatPercent,
        LineTotal = line.LineAmount,
    };
}
