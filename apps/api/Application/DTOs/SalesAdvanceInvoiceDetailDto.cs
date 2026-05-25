namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Detail view for a sales advance (proforma) invoice — header, line items, computed
/// totals, plus a payment-tracking section that the regular sales-invoice detail does
/// not carry. Reuses the regular sales-invoice header/line/totals DTOs (DRY) since the
/// advance-invoice document shares the standard BC sales-invoice shape; only the
/// payment-tracking block is advance-specific.
///
/// NOTE (Q-003): the advance-invoice format is implemented against the STANDARD BC
/// advance-invoice schema pending client confirmation (a BiH/SRB localized format is
/// possible). The shape is isolated behind <see cref="Mapping.IBcMapper{TSource,TTarget}"/>
/// so a localized format is a low-cost mapper + mock swap.
/// </summary>
public class SalesAdvanceInvoiceDetailDto
{
    public SalesInvoiceHeaderDto Header { get; set; } = new();
    public List<SalesInvoiceLineDto> Lines { get; set; } = new();
    public SalesInvoiceTotalsDto Totals { get; set; } = new();
    public PaymentTrackingDto PaymentTracking { get; set; } = new();
}

/// <summary>
/// Advance-payment tracking for an advance invoice. Invariant: <see cref="Amount"/>
/// equals <see cref="AmountPaid"/> + <see cref="Remaining"/>.
/// </summary>
public class PaymentTrackingDto
{
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Remaining { get; set; }
}
