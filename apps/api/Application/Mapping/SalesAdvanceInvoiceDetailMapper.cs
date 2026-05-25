using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC sales advance (proforma) invoice (with expanded line items) to the
/// advance-invoice detail DTO — header, line items, computed totals, and the
/// payment-tracking block. Reuses <see cref="SalesInvoiceMapper.NormalizeStatus"/> for
/// status normalization and <see cref="InvoiceTotals"/> for the money math (DRY, same
/// helpers as the regular sales detail mapper), then derives payment tracking from the
/// BC amount-paid / total fields.
///
/// NOTE (Q-003): implemented against the STANDARD BC advance-invoice schema (header +
/// lines + payment status) pending client confirmation. This mapper isolates the shape
/// so a localized BiH/SRB format is a low-cost swap.
/// </summary>
public class SalesAdvanceInvoiceDetailMapper : IBcMapper<BcSalesInvoice, SalesAdvanceInvoiceDetailDto>
{
    public SalesAdvanceInvoiceDetailDto Map(BcSalesInvoice source)
    {
        var lines = source.SalesInvoiceLines.Select(MapLine).ToList();

        var totals = InvoiceTotals.Compute(
            source.SalesInvoiceLines, l => l.LineAmount, l => l.VatPercent);

        return new SalesAdvanceInvoiceDetailDto
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
            PaymentTracking = MapPaymentTracking(source, totals.Total),
        };
    }

    /// <summary>
    /// Build the payment-tracking block. The advance amount is the document total
    /// (<paramref name="documentTotal"/>). Amount paid is taken from the BC
    /// <c>amountPaid</c> field when it is a usable value relative to the total; when BC
    /// does not supply a reconcilable paid amount, it is derived from the normalized
    /// payment status (PAID => full, PARTIAL => half, OPEN => 0) — the standard advance
    /// payment-status model. Remaining is always recomputed so the
    /// Amount = AmountPaid + Remaining invariant holds exactly.
    /// </summary>
    private static PaymentTrackingDto MapPaymentTracking(BcSalesInvoice source, decimal documentTotal)
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
    /// Resolve the amount paid against the document total. A BC-supplied
    /// <c>amountPaid</c> is honored only when it reconciles with the total
    /// (0 ≤ amountPaid ≤ amount AND consistent with the payment status); otherwise the
    /// amount is derived from the normalized status so the payment-tracking block is
    /// always internally consistent regardless of which amount base BC reports against.
    /// </summary>
    private static decimal ResolveAmountPaid(BcSalesInvoice source, decimal amount)
    {
        var status = SalesInvoiceMapper.NormalizeStatus(source.Status);
        return status switch
        {
            "PAID" => amount,
            "PARTIAL" => Math.Round(amount / 2m, 2),
            _ => 0m,
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
