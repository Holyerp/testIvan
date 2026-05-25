namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Detail view for a purchase advance (proforma) invoice — header, line items, computed
/// totals, plus a payment-tracking section that the regular purchase-invoice detail does
/// not carry. Reuses the regular purchase-invoice header/line/totals DTOs (DRY) since the
/// advance-invoice document shares the standard BC purchase-invoice shape; only the
/// payment-tracking block is advance-specific. Reuses the generic
/// <see cref="PaymentTrackingDto"/> introduced for the sales advance invoice (US-014).
/// Mirrors <see cref="SalesAdvanceInvoiceDetailDto"/> on the vendor side.
///
/// NOTE (Q-003): the advance-invoice format is implemented against the STANDARD BC
/// advance-invoice schema pending client confirmation (a BiH/SRB localized format is
/// possible). The shape is isolated behind <see cref="Mapping.IBcMapper{TSource,TTarget}"/>
/// so a localized format is a low-cost mapper + mock swap.
/// </summary>
public class PurchaseAdvanceInvoiceDetailDto
{
    public PurchaseInvoiceHeaderDto Header { get; set; } = new();
    public List<PurchaseInvoiceLineDto> Lines { get; set; } = new();
    public PurchaseInvoiceTotalsDto Totals { get; set; } = new();
    public PaymentTrackingDto PaymentTracking { get; set; } = new();
}
