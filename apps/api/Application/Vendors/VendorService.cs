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
    private readonly IBcMapper<BcPurchaseInvoice, PurchaseInvoiceListItemDto> _purchaseInvoiceMapper;

    public VendorService(
        IBcHttpClient bc,
        IBcMapper<BcVendor, VendorListItemDto> vendorMapper,
        IBcMapper<BcPurchaseInvoice, PurchaseInvoiceListItemDto> purchaseInvoiceMapper)
    {
        _bc = bc;
        _vendorMapper = vendorMapper;
        _purchaseInvoiceMapper = purchaseInvoiceMapper;
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

    public async Task<VendorDetailDto?> GetVendorByIdAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var vendor = await _bc.GetByIdAsync<BcVendor>("vendors", id, cancellationToken: cancellationToken);
        if (vendor == null || string.IsNullOrEmpty(vendor.Id)) return null;

        var invoices = await GetVendorInvoicesAsync(vendor.DisplayName, cancellationToken);

        return new VendorDetailDto
        {
            Vendor = new VendorProfileDto
            {
                Id = vendor.Id,
                Number = vendor.Number,
                DisplayName = vendor.DisplayName,
                Address = vendor.Address,
                City = vendor.City,
                Phone = vendor.Phone,
                Email = vendor.Email,
                VatNumber = vendor.VatNumber,
                Balance = vendor.Balance,
                PaymentTerms = vendor.PaymentTerms,
            },
            Invoices = invoices,
        };
    }

    // Last 20 posted purchase invoices for the given vendor. Mirrors
    // CustomerService.GetCustomerByIdAsync's invoice fetch; reuses the shared
    // PurchaseInvoiceMapper for the row mapping (DRY). Returns null when the vendor
    // does not exist, so the /{id}/invoices endpoint can map that to a 404.
    public async Task<List<PurchaseInvoiceListItemDto>?> GetVendorInvoicesForEndpointAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var vendor = await _bc.GetByIdAsync<BcVendor>("vendors", id, cancellationToken: cancellationToken);
        if (vendor == null || string.IsNullOrEmpty(vendor.Id)) return null;

        return await GetVendorInvoicesAsync(vendor.DisplayName, cancellationToken);
    }

    private async Task<List<PurchaseInvoiceListItemDto>> GetVendorInvoicesAsync(
        string vendorName,
        CancellationToken cancellationToken)
    {
        // Escape single quotes per OData string-literal rules to avoid filter injection.
        var safeName = vendorName.Replace("'", "''");
        var invoicesResult = await _bc.GetCollectionAsync<BcPurchaseInvoice>(
            "purchaseInvoicesPosted",
            new BcQueryOptions
            {
                Filter = $"vendorName eq '{safeName}'",
                OrderBy = "postingDate desc",
                Top = 20,
            },
            cancellationToken);

        return invoicesResult.Value.Select(_purchaseInvoiceMapper.Map).ToList();
    }
}
