using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC purchase invoice to the list-item DTO the UI consumes.
/// Normalizes the BC-style status casing into the Pinoles cross-layer wire value
/// (SCREAMING_SNAKE_CASE: OPEN | PARTIAL | PAID) via the shared
/// <see cref="InvoiceStatus"/> helper. Mirrors <see cref="SalesInvoiceMapper"/> but
/// carries a vendor instead of a customer.
/// </summary>
public class PurchaseInvoiceMapper : IBcMapper<BcPurchaseInvoice, PurchaseInvoiceListItemDto>
{
    public PurchaseInvoiceListItemDto Map(BcPurchaseInvoice source) => new()
    {
        Id = source.Id,
        Number = source.Number,
        VendorName = source.VendorName,
        PostingDate = source.PostingDate,
        DueDate = source.DueDate,
        Amount = source.TotalAmountIncludingTax,
        Status = InvoiceStatus.Normalize(source.Status),
    };
}
