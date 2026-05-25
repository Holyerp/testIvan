using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Domain.Constants;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC correction document to the unified credit-documents list-item DTO
/// (US-016). Normalizes the document Type to the SCREAMING_SNAKE wire value
/// (CREDIT_MEMO | DEBIT_MEMO | STORNO) and the Status via the shared
/// <see cref="InvoiceStatus"/> credit-memo lifecycle (OPEN | POSTED) so the wire values
/// match .claude/rules/enums-and-constants.md. Unknown types default to CREDIT_MEMO.
/// </summary>
public class CreditDocumentMapper : IBcMapper<BcCreditDocument, CreditDocumentListItemDto>
{
    public CreditDocumentListItemDto Map(BcCreditDocument source) => new()
    {
        Id = source.Id,
        Number = source.Number,
        Type = NormalizeType(source.Type),
        PartyName = source.PartyName,
        PostingDate = source.PostingDate,
        Amount = source.TotalAmountIncludingTax,
        OriginalInvoiceNumber = source.OriginalInvoiceNumber,
        Status = InvoiceStatus.NormalizeCreditMemo(source.Status),
    };

    /// <summary>
    /// Normalize a BC document-type string to the Pinoles wire value
    /// (CREDIT_MEMO | DEBIT_MEMO | STORNO). Accepts already-wire values and common
    /// BC-style casings; unknown values default to CREDIT_MEMO.
    /// </summary>
    public static string NormalizeType(string? bcType)
    {
        var normalized = (bcType ?? string.Empty).Trim().ToUpperInvariant();
        return normalized switch
        {
            "DEBIT_MEMO" or "DEBITMEMO" or "DEBIT MEMO" => CreditDocumentType.DebitMemo,
            "STORNO" or "CANCELLATION" => CreditDocumentType.Storno,
            "CREDIT_MEMO" or "CREDITMEMO" or "CREDIT MEMO" => CreditDocumentType.CreditMemo,
            _ => CreditDocumentType.CreditMemo,
        };
    }
}
