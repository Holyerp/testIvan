using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC sales invoice to the list-item DTO the UI consumes.
/// Normalizes the BC-style status casing (e.g. "Open", "Partially Paid", "Paid")
/// into the Pinoles cross-layer wire value (SCREAMING_SNAKE_CASE: OPEN | PARTIAL | PAID)
/// per .claude/rules/enums-and-constants.md. Unknown statuses default to OPEN.
/// </summary>
public class SalesInvoiceMapper : IBcMapper<BcSalesInvoice, SalesInvoiceListItemDto>
{
    public SalesInvoiceListItemDto Map(BcSalesInvoice source) => new()
    {
        Id = source.Id,
        Number = source.Number,
        CustomerName = source.CustomerName,
        PostingDate = source.PostingDate,
        DueDate = source.DueDate,
        Amount = source.TotalAmountIncludingTax,
        Status = NormalizeStatus(source.Status),
    };

    /// <summary>Normalize a BC status string to the Pinoles wire value.</summary>
    public static string NormalizeStatus(string? bcStatus)
    {
        var normalized = (bcStatus ?? string.Empty).Trim().ToLowerInvariant();
        return normalized switch
        {
            "paid" => "PAID",
            "partial" or "partially paid" => "PARTIAL",
            "open" => "OPEN",
            _ => "OPEN",
        };
    }
}
