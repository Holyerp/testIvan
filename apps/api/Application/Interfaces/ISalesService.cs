using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

public interface ISalesService
{
    Task<PagedResultDto<SalesInvoiceListItemDto>> GetInvoicesAsync(
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
}
