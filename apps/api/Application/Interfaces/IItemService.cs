using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

public interface IItemService
{
    Task<PagedResultDto<ItemListItemDto>> GetItemsAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        string? category,
        string? location,
        CancellationToken cancellationToken = default);

    // Item detail (US-018): full profile + stock by location + the most recent ledger
    // entries. Returns null when the item does not exist (→ 404).
    Task<ItemDetailDto?> GetItemByIdAsync(
        string id,
        CancellationToken cancellationToken = default);

    // Item ledger entries for the dedicated /{id}/ledger-entries endpoint. Returns null
    // when the item does not exist (→ 404), otherwise the last <paramref name="top"/>
    // movements (newest first, possibly empty).
    Task<List<ItemLedgerEntryDto>?> GetItemLedgerEntriesAsync(
        string id,
        int top = 20,
        CancellationToken cancellationToken = default);
}
