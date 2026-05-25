namespace Pinoles.Api.Application.Common;

/// <summary>
/// Shared computation of invoice/credit-memo totals from line items. Single source of
/// truth used by both the sales detail mapper (US-007/008) and the purchase detail
/// mapper (US-010) so the OPEN/PARTIAL/PAID-independent money math never diverges.
///
///   subtotal  = Σ lineAmount
///   vatAmount = Σ (lineAmount × vatPercent / 100)
///   total     = subtotal + vatAmount
/// </summary>
public static class InvoiceTotals
{
    public readonly record struct Result(decimal Subtotal, decimal VatAmount, decimal Total);

    /// <summary>
    /// Compute totals from a sequence of (lineAmount, vatPercent) pairs. Each layer
    /// passes its own line shape via the two selectors so the helper stays decoupled
    /// from sales-/purchase-specific line types (Dependency Inversion).
    /// </summary>
    public static Result Compute<TLine>(
        IEnumerable<TLine> lines,
        Func<TLine, decimal> lineAmount,
        Func<TLine, decimal> vatPercent)
    {
        var subtotal = 0m;
        var vatAmount = 0m;
        foreach (var line in lines)
        {
            var amount = lineAmount(line);
            subtotal += amount;
            vatAmount += amount * vatPercent(line) / 100m;
        }

        return new Result(subtotal, vatAmount, subtotal + vatAmount);
    }
}
