namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// One row of the unified credit-documents list (US-016): credit memos, debit memos,
/// and storno (cancellation) invoices. <see cref="Type"/> distinguishes the document
/// type (SCREAMING_SNAKE wire value: CREDIT_MEMO | DEBIT_MEMO | STORNO) and
/// <see cref="OriginalInvoiceNumber"/> references the invoice the correction applies to.
/// </summary>
public class CreditDocumentListItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;          // CREDIT_MEMO | DEBIT_MEMO | STORNO
    public string PartyName { get; set; } = string.Empty;     // customer or vendor name
    public string PostingDate { get; set; } = string.Empty;   // ISO yyyy-MM-dd
    public decimal Amount { get; set; }
    public string OriginalInvoiceNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;        // OPEN | POSTED (SCREAMING_SNAKE wire format)
}
