using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

public interface IPurchaseService
{
    Task<PagedResultDto<PurchaseInvoiceListItemDto>> GetInvoicesAsync(
        string entitySet,
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        string? status,
        string? fromDate,
        string? toDate,
        CancellationToken ct = default);

    Task<PurchaseInvoiceDetailDto?> GetInvoiceByIdAsync(
        string entitySet,
        string id,
        CancellationToken ct = default);
}
