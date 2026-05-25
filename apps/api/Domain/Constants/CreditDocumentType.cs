namespace Pinoles.Api.Domain.Constants;

/// <summary>
/// Cross-layer document-type enum for correction documents (US-016). The unified
/// credit-documents list mixes three document types; the type is a cross-layer enum
/// so its wire VALUE is SCREAMING_SNAKE_CASE per .claude/rules/enums-and-constants.md.
/// The frontend maps these values to i18n labels; the allowed values are documented
/// in docs/api/credit-documents.md.
/// </summary>
public static class CreditDocumentType
{
    public const string CreditMemo = "CREDIT_MEMO";
    public const string DebitMemo = "DEBIT_MEMO";
    public const string Storno = "STORNO";

    public static readonly string[] All = { CreditMemo, DebitMemo, Storno };

    public static bool IsValid(string type) => All.Contains(type);
}
