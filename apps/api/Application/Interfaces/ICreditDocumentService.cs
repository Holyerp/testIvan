using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

/// <summary>
/// Serves the unified credit-documents list + detail (US-016): credit memos, debit
/// memos, and storno (cancellation) invoices across one collection. The list supports
/// a <c>type</c> filter (CREDIT_MEMO | DEBIT_MEMO | STORNO) in addition to the standard
/// search and posting-date range.
/// </summary>
public interface ICreditDocumentService
{
    Task<PagedResultDto<CreditDocumentListItemDto>> GetCreditDocumentsAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        string? type,
        string? fromDate,
        string? toDate,
        CancellationToken ct = default);

    Task<CreditDocumentDetailDto?> GetCreditDocumentByIdAsync(
        string id,
        CancellationToken ct = default);
}
