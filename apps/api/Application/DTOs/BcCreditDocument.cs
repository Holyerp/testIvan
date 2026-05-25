namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Raw BC correction document (US-016): a credit memo, debit memo, or storno invoice.
/// Shares the invoice header/line/totals shape but carries a <see cref="Type"/>
/// discriminator and an <see cref="OriginalInvoiceNumber"/> reference to the document
/// the correction applies to. Lines are loaded via the salesInvoiceLines $expand
/// navigation property (reused from the sales-invoice schema).
/// </summary>
public class BcCreditDocument
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;          // BC-style; normalized to wire value by the mapper
    public string PartyName { get; set; } = string.Empty;
    public string PostingDate { get; set; } = string.Empty;
    public decimal TotalAmountIncludingTax { get; set; }
    public string OriginalInvoiceNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    // Line items, loaded via the salesInvoiceLines $expand navigation property
    // (same shape as a sales invoice line — DRY).
    public List<BcSalesInvoiceLine> SalesInvoiceLines { get; set; } = new();
}
