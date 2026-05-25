using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

public interface IVendorService
{
    Task<PagedResultDto<VendorListItemDto>> GetVendorsAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        CancellationToken cancellationToken = default);

    Task<VendorDetailDto?> GetVendorByIdAsync(
        string id,
        CancellationToken cancellationToken = default);

    // Vendor purchase history for the dedicated /{id}/invoices endpoint. Returns null
    // when the vendor does not exist (→ 404), otherwise the last 20 posted purchase
    // invoices (possibly empty).
    Task<List<PurchaseInvoiceListItemDto>?> GetVendorInvoicesForEndpointAsync(
        string id,
        CancellationToken cancellationToken = default);
}
