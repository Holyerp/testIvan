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
}
