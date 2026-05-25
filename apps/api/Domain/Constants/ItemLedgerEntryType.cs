namespace Pinoles.Api.Domain.Constants;

/// <summary>
/// Cross-layer enum for warehouse item ledger (stock movement) entry types (US-018).
/// Mirrors <see cref="CreditDocumentType"/>: the wire VALUE is SCREAMING_SNAKE_CASE per
/// .claude/rules/enums-and-constants.md. <see cref="Normalize"/> coerces raw BC casing
/// (e.g. "Purchase", "Sale") to the canonical wire value so the mapper produces one
/// stable form. The frontend maps these values to i18n labels
/// (items.ledgerType.&lt;VALUE&gt;); the allowed values are documented in docs/api/items.md.
/// </summary>
public static class ItemLedgerEntryType
{
    public const string Purchase = "PURCHASE";
    public const string Sale = "SALE";
    public const string Adjustment = "ADJUSTMENT";
    public const string Transfer = "TRANSFER";

    public static readonly string[] All = { Purchase, Sale, Adjustment, Transfer };

    public static bool IsValid(string type) => All.Contains(type);

    /// <summary>
    /// Normalize a raw BC entry-type string to the canonical SCREAMING_SNAKE wire value.
    /// Maps BC's PascalCase / mixed-case forms (Purchase, Sale, Adjustment, Transfer) to
    /// the wire value; unknown values fall back to <see cref="Adjustment"/> so the UI
    /// always receives a documented enum member rather than an arbitrary string.
    /// </summary>
    public static string Normalize(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return Adjustment;
        var upper = raw.Trim().ToUpperInvariant();
        return IsValid(upper) ? upper : Adjustment;
    }
}
