using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Items;

/// <summary>
/// Warehouse item list service (US-017). Mirrors <see cref="Vendors.VendorService"/>:
/// builds a <see cref="BcListQuery"/>, queries the "items" entity set, and projects with
/// <see cref="ItemMapper"/>. Adds category/location equality filters on top of the shared
/// number/description contains-search. This is the first WAREHOUSE-module list service.
/// </summary>
public class ItemService : IItemService
{
    // BC field names (what $orderby uses). The first entry is the default sort.
    private static readonly string[] AllowedSortFields = { "description", "quantityOnHand", "unitCost" };

    private readonly IBcHttpClient _bc;
    private readonly IBcMapper<BcItem, ItemListItemDto> _itemMapper;

    public ItemService(IBcHttpClient bc, IBcMapper<BcItem, ItemListItemDto> itemMapper)
    {
        _bc = bc;
        _itemMapper = itemMapper;
    }

    public async Task<PagedResultDto<ItemListItemDto>> GetItemsAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        string? category,
        string? location,
        CancellationToken cancellationToken = default)
    {
        // Map the UI sort key (name/quantity/unitCost) onto the BC field name the
        // OrderBy clause uses, so the public contract stays UI-friendly while the
        // allow-list below is enforced against the wire field names.
        var bcSortBy = MapSortField(sortBy);

        var options = BcListQuery.Build(
            page,
            pageSize,
            bcSortBy,
            sortDir,
            AllowedSortFields,
            search,
            term => $"contains(number,'{term}') or contains(description,'{term}')");

        // Append category/location equality filters (escaping single quotes per OData
        // string-literal rules). Combined with the search clause via AND when both present.
        var extraFilters = new List<string>();
        if (!string.IsNullOrWhiteSpace(category))
            extraFilters.Add($"category eq '{category.Replace("'", "''")}'");
        if (!string.IsNullOrWhiteSpace(location))
            extraFilters.Add($"location eq '{location.Replace("'", "''")}'");

        if (extraFilters.Count > 0)
        {
            var combined = string.Join(" and ", extraFilters);
            options.Filter = string.IsNullOrWhiteSpace(options.Filter)
                ? combined
                : $"({options.Filter}) and {combined}";
        }

        var result = await _bc.GetCollectionAsync<BcItem>("items", options, cancellationToken);

        var items = result.Value.Select(_itemMapper.Map).ToList();

        // Reuse the clamped values BcListQuery produced so the result echoes the
        // effective page/pageSize rather than the raw (possibly invalid) request.
        var effectivePage = (options.Skip ?? 0) / (options.Top ?? BcListQuery.DefaultPageSize) + 1;
        var effectivePageSize = options.Top ?? BcListQuery.DefaultPageSize;

        return new PagedResultDto<ItemListItemDto>
        {
            Items = items,
            Total = result.Count ?? items.Count,
            Page = effectivePage,
            PageSize = effectivePageSize,
        };
    }

    // UI sort key → BC field name. Unknown values pass through untouched and are then
    // rejected to the default by BcListQuery's allow-list.
    private static string? MapSortField(string? sortBy) => sortBy switch
    {
        "name" => "description",
        "quantity" => "quantityOnHand",
        "unitCost" => "unitCost",
        _ => sortBy,
    };
}
