namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Detail view of a single correction document (US-016) — header, line items, totals.
/// Reuses <see cref="SalesInvoiceLineDto"/> and <see cref="SalesInvoiceTotalsDto"/>
/// (the line/totals shape is identical to a sales invoice; DRY). The header adds the
/// document <see cref="CreditDocumentHeaderDto.Type"/> and
/// <see cref="CreditDocumentHeaderDto.OriginalInvoiceNumber"/> reference.
/// </summary>
public class CreditDocumentDetailDto
{
    public CreditDocumentHeaderDto Header { get; set; } = new();
    public List<SalesInvoiceLineDto> Lines { get; set; } = new();
    public SalesInvoiceTotalsDto Totals { get; set; } = new();
}

public class CreditDocumentHeaderDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;          // CREDIT_MEMO | DEBIT_MEMO | STORNO
    public string PartyName { get; set; } = string.Empty;     // customer or vendor name
    public string PostingDate { get; set; } = string.Empty;   // ISO yyyy-MM-dd
    public string OriginalInvoiceNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;        // OPEN | POSTED (SCREAMING_SNAKE wire format)
}
