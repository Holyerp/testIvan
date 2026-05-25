using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Vendors;

public class VendorService : IVendorService
{
    private static readonly string[] AllowedSortFields = { "displayName", "balance" };

    private readonly IBcHttpClient _bc;
    private readonly IBcMapper<BcVendor, VendorListItemDto> _vendorMapper;

    public VendorService(
        IBcHttpClient bc,
        IBcMapper<BcVendor, VendorListItemDto> vendorMapper)
    {
        _bc = bc;
        _vendorMapper = vendorMapper;
    }

    public async Task<PagedResultDto<VendorListItemDto>> GetVendorsAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        CancellationToken cancellationToken = default)
    {
        var options = BcListQuery.Build(
            page,
            pageSize,
            sortBy,
            sortDir,
            AllowedSortFields,
            search,
            term => $"contains(displayName,'{term}') or contains(number,'{term}')");

        var result = await _bc.GetCollectionAsync<BcVendor>("vendors", options, cancellationToken);

        var items = result.Value.Select(_vendorMapper.Map).ToList();

        // Reuse the clamped values BcListQuery produced so the result echoes the
        // effective page/pageSize rather than the raw (possibly invalid) request.
        var effectivePage = (options.Skip ?? 0) / (options.Top ?? BcListQuery.DefaultPageSize) + 1;
        var effectivePageSize = options.Top ?? BcListQuery.DefaultPageSize;

        return new PagedResultDto<VendorListItemDto>
        {
            Items = items,
            Total = result.Count ?? items.Count,
            Page = effectivePage,
            PageSize = effectivePageSize,
        };
    }
}
