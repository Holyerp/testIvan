using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC purchase advance (proforma) invoice (with expanded line items) to the
/// advance-invoice detail DTO — header, line items, computed totals, and the
/// payment-tracking block. Reuses the shared <see cref="InvoiceStatus.Normalize"/> for
/// status normalization and <see cref="InvoiceTotals"/> for the money math (DRY, same
/// helpers as the regular purchase detail mapper), then derives payment tracking from the
/// normalized payment status. Mirrors <see cref="SalesAdvanceInvoiceDetailMapper"/> on
/// the vendor side.
///
/// NOTE (Q-003): implemented against the STANDARD BC advance-invoice schema (header +
/// lines + payment status) pending client confirmation. This mapper isolates the shape
/// so a localized BiH/SRB format is a low-cost swap.
/// </summary>
public class PurchaseAdvanceInvoiceDetailMapper : IBcMapper<BcPurchaseInvoice, PurchaseAdvanceInvoiceDetailDto>
{
    public PurchaseAdvanceInvoiceDetailDto Map(BcPurchaseInvoice source)
    {
        var lines = source.PurchaseInvoiceLines.Select(MapLine).ToList();

        var totals = InvoiceTotals.Compute(
            source.PurchaseInvoiceLines, l => l.LineAmount, l => l.VatPercent);

        return new PurchaseAdvanceInvoiceDetailDto
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
            PaymentTracking = MapPaymentTracking(source, totals.Total),
        };
    }

    /// <summary>
    /// Build the payment-tracking block. The advance amount is the document total
    /// (<paramref name="documentTotal"/>). Amount paid is derived from the normalized
    /// payment status (PAID => full, PARTIAL => half, OPEN => 0) — the standard advance
    /// payment-status model. Remaining is always recomputed so the
    /// Amount = AmountPaid + Remaining invariant holds exactly.
    /// </summary>
    private static PaymentTrackingDto MapPaymentTracking(BcPurchaseInvoice source, decimal documentTotal)
    {
        var amount = documentTotal;
        var amountPaid = ResolveAmountPaid(source, amount);

        return new PaymentTrackingDto
        {
            Amount = amount,
            AmountPaid = amountPaid,
            Remaining = amount - amountPaid,
        };
    }

    /// <summary>
    /// Resolve the amount paid against the document total from the normalized status so
    /// the payment-tracking block is always internally consistent regardless of which
    /// amount base BC reports against (PAID => full, PARTIAL => half, OPEN => 0).
    /// </summary>
    private static decimal ResolveAmountPaid(BcPurchaseInvoice source, decimal amount)
    {
        var status = InvoiceStatus.Normalize(source.Status);
        return status switch
        {
            "PAID" => amount,
            "PARTIAL" => Math.Round(amount / 2m, 2),
            _ => 0m,
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
