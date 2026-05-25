using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC purchase invoice (with expanded line items) to the detail DTO the UI
/// consumes — header, line items, and computed totals. Reuses the shared
/// <see cref="InvoiceStatus.Normalize"/> for the OPEN/PARTIAL/PAID wire value and
/// <see cref="InvoiceTotals"/> for the money math, so purchase and sales detail stay
/// consistent (DRY). Mirrors <see cref="SalesInvoiceDetailMapper"/> but carries a vendor.
/// </summary>
public class PurchaseInvoiceDetailMapper : IBcMapper<BcPurchaseInvoice, PurchaseInvoiceDetailDto>
{
    public PurchaseInvoiceDetailDto Map(BcPurchaseInvoice source)
    {
        var lines = source.PurchaseInvoiceLines.Select(MapLine).ToList();

        var totals = InvoiceTotals.Compute(
            source.PurchaseInvoiceLines, l => l.LineAmount, l => l.VatPercent);

        return new PurchaseInvoiceDetailDto
        {
            Header = new PurchaseInvoiceHeaderDto
            {
                Id = source.Id,
                Number = source.Number,
                VendorName = source.VendorName,
                PostingDate = source.PostingDate,
                DueDate = source.DueDate,
                PaymentTerms = source.PaymentTerms,
                OurReference = source.OurReference,
                Status = InvoiceStatus.Normalize(source.Status),
            },
            Lines = lines,
            Totals = new PurchaseInvoiceTotalsDto
            {
                Subtotal = totals.Subtotal,
                VatAmount = totals.VatAmount,
                Total = totals.Total,
            },
        };
    }

    private static PurchaseInvoiceLineDto MapLine(BcPurchaseInvoiceLine line) => new()
    {
        Description = line.Description,
        Quantity = line.Quantity,
        UnitPrice = line.UnitPrice,
        VatPercent = line.VatPercent,
        LineTotal = line.LineAmount,
    };
}
