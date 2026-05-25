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
}
