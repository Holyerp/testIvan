namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Shared normalization of BC-style invoice/credit-memo status strings into the
/// Pinoles cross-layer wire values (SCREAMING_SNAKE_CASE) per
/// .claude/rules/enums-and-constants.md. Single source of truth used by both the
/// sales mappers (US-006..US-008) and the purchase mappers (US-009..US-010).
/// </summary>
public static class InvoiceStatus
{
    /// <summary>
    /// Normalize a BC invoice status string to the wire value (OPEN | PARTIAL | PAID).
    /// Unknown statuses default to OPEN. Additive: also recognizes "posted" so data
    /// passing through here is never lost, but credit memos should use
    /// <see cref="NormalizeCreditMemo"/>.
    /// </summary>
    public static string Normalize(string? bcStatus)
    {
        var normalized = (bcStatus ?? string.Empty).Trim().ToLowerInvariant();
        return normalized switch
        {
            "paid" => "PAID",
            "partial" or "partially paid" => "PARTIAL",
            "posted" => "POSTED",
            "open" => "OPEN",
            _ => "OPEN",
        };
    }

    /// <summary>
    /// Normalize a BC credit-memo status string to the wire value. Credit memos use
    /// OPEN | POSTED (not the PARTIAL/PAID invoice lifecycle). Unknown statuses
    /// default to OPEN. Per .claude/rules/enums-and-constants.md.
    /// </summary>
    public static string NormalizeCreditMemo(string? bcStatus)
    {
        var normalized = (bcStatus ?? string.Empty).Trim().ToLowerInvariant();
        return normalized switch
        {
            "posted" => "POSTED",
            "open" => "OPEN",
            _ => "OPEN",
        };
    }
}
