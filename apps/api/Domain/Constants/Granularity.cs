namespace Pinoles.Api.Domain.Constants;

/// <summary>
/// Cross-layer enum for the analytics revenue/expense time-series granularity (US-020).
/// The wire VALUE is SCREAMING_SNAKE_CASE per .claude/rules/enums-and-constants.md.
/// <see cref="Normalize"/> coerces an arbitrary/missing query value to a valid member,
/// defaulting to <see cref="Monthly"/>. The frontend maps these to i18n labels
/// (analytics.granularity.&lt;VALUE&gt;); allowed values are documented in
/// docs/api/analytics.md.
/// </summary>
public static class Granularity
{
    public const string Monthly = "MONTHLY";
    public const string Quarterly = "QUARTERLY";
    public const string Yearly = "YEARLY";

    public static readonly string[] All = { Monthly, Quarterly, Yearly };

    public static bool IsValid(string granularity) => All.Contains(granularity);

    /// <summary>
    /// Normalize a raw query value to a valid granularity wire value. Unknown / missing
    /// values fall back to <see cref="Monthly"/> so the service always buckets against a
    /// documented enum member rather than an arbitrary string.
    /// </summary>
    public static string Normalize(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return Monthly;
        var upper = raw.Trim().ToUpperInvariant();
        return IsValid(upper) ? upper : Monthly;
    }
}
