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

    Task<SalesInvoiceDetailDto?> GetInvoiceByIdAsync(
        string entitySet,
        string id,
        CancellationToken ct = default);

    /// <summary>
    /// Advance (proforma) invoice detail — header, line items, computed totals, plus a
    /// payment-tracking block (amount / amount paid / remaining). Reads the
    /// <c>salesAdvanceInvoices</c> BC collection. Returns null when the id is unknown.
    /// </summary>
    Task<SalesAdvanceInvoiceDetailDto?> GetAdvanceInvoiceByIdAsync(
        string id,
        CancellationToken ct = default);
}
