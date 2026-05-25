using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC vendor entity to the vendor list-item DTO the UI consumes.
/// Mirrors <see cref="CustomerMapper"/> (the reference implementation of the pattern).
/// </summary>
public class VendorMapper : IBcMapper<BcVendor, VendorListItemDto>
{
    public VendorListItemDto Map(BcVendor source) => new()
    {
        Id = source.Id,
        Number = source.Number,
        DisplayName = source.DisplayName,
        City = source.City,
        Balance = source.Balance,
        Phone = source.Phone,
    };
}
