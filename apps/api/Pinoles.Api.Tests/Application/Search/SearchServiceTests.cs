using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.Customers;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Application.Purchase;
using Pinoles.Api.Application.Sales;
using Pinoles.Api.Application.Search;
using Pinoles.Api.Application.Vendors;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Application.Search;

public class SearchServiceTests
{
    private static readonly string[] FinancialRole = { UserRoles.Accounting };
    private static readonly string[] WarehouseRole = { UserRoles.Warehouse };

    // Build the SearchService backed by the real list services + MockBcHttpClient,
    // mirroring how the per-service tests wire their dependencies.
    private static SearchService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        var customers = new CustomerService(bc, new CustomerMapper());
        var vendors = new VendorService(bc, new VendorMapper(), new PurchaseInvoiceMapper());
        var sales = new SalesService(bc, new SalesInvoiceMapper(), new SalesInvoiceDetailMapper(), new SalesAdvanceInvoiceDetailMapper());
        var purchase = new PurchaseService(bc, new PurchaseInvoiceMapper(), new PurchaseInvoiceDetailMapper());
        return new SearchService(customers, vendors, sales, purchase);
    }

    [Fact]
    public async Task SearchAsync_FinancialRole_ReturnsHitsAcrossGroups()
    {
        // "a" is a 1-char term → too short. Use a 2+ char common substring.
        var result = await CreateService().SearchAsync("00", 5, FinancialRole);

        // At least one of the four groups should produce hits for a broad term.
        var total = result.Customers.Count + result.Vendors.Count
            + result.SalesInvoices.Count + result.PurchaseInvoices.Count;
        Assert.True(total > 0);
    }

    [Fact]
    public async Task SearchAsync_MatchesAllFourGroups_ForBroadTerm()
    {
        // "01" appears in every entity number: C001.., V001.., SI-001.., PI-001..
        var result = await CreateService().SearchAsync("01", 20, FinancialRole);

        Assert.NotEmpty(result.Customers);
        Assert.NotEmpty(result.Vendors);
        Assert.NotEmpty(result.SalesInvoices);
        Assert.NotEmpty(result.PurchaseInvoices);
    }

    [Fact]
    public async Task SearchAsync_RespectsLimitPerGroup()
    {
        const int limit = 2;
        var result = await CreateService().SearchAsync("01", limit, FinancialRole);

        Assert.True(result.Customers.Count <= limit);
        Assert.True(result.Vendors.Count <= limit);
        Assert.True(result.SalesInvoices.Count <= limit);
        Assert.True(result.PurchaseInvoices.Count <= limit);
    }

    [Fact]
    public async Task SearchAsync_ClampsLimitAboveMax()
    {
        // limit 9999 clamps to 20 → no group can exceed 20.
        var result = await CreateService().SearchAsync("01", 9999, FinancialRole);

        Assert.True(result.SalesInvoices.Count <= 20);
        Assert.True(result.PurchaseInvoices.Count <= 20);
    }

    [Fact]
    public async Task SearchAsync_WarehouseRole_ReturnsAllEmptyGroups()
    {
        var result = await CreateService().SearchAsync("01", 5, WarehouseRole);

        Assert.Empty(result.Customers);
        Assert.Empty(result.Vendors);
        Assert.Empty(result.SalesInvoices);
        Assert.Empty(result.PurchaseInvoices);
    }

    [Fact]
    public async Task SearchAsync_ShortQuery_ReturnsEmptyGroups()
    {
        var result = await CreateService().SearchAsync("a", 5, FinancialRole);

        Assert.Empty(result.Customers);
        Assert.Empty(result.Vendors);
        Assert.Empty(result.SalesInvoices);
        Assert.Empty(result.PurchaseInvoices);
    }

    [Fact]
    public async Task SearchAsync_EmptyQuery_ReturnsEmptyGroups()
    {
        var result = await CreateService().SearchAsync("   ", 5, FinancialRole);

        Assert.Empty(result.Customers);
        Assert.Empty(result.Vendors);
    }

    [Fact]
    public async Task SearchAsync_SalesInvoiceHit_MapsTitleSubtitleType()
    {
        var result = await CreateService().SearchAsync("SI-001", 5, FinancialRole);

        var hit = Assert.Single(result.SalesInvoices, h => h.Title == "SI-001");
        Assert.Equal("SALES_INVOICE", hit.Type);
        Assert.False(string.IsNullOrEmpty(hit.Id));
        Assert.False(string.IsNullOrEmpty(hit.Subtitle)); // customer name
    }

    [Fact]
    public async Task SearchAsync_PurchaseInvoiceHit_MapsTitleSubtitleType()
    {
        var result = await CreateService().SearchAsync("PI-001", 5, FinancialRole);

        var hit = Assert.Single(result.PurchaseInvoices, h => h.Title == "PI-001");
        Assert.Equal("PURCHASE_INVOICE", hit.Type);
        Assert.False(string.IsNullOrEmpty(hit.Id));
        Assert.False(string.IsNullOrEmpty(hit.Subtitle)); // vendor name
    }

    [Fact]
    public async Task SearchAsync_VendorHit_MapsTitleSubtitleType()
    {
        var result = await CreateService().SearchAsync("V001", 5, FinancialRole);

        var hit = Assert.Single(result.Vendors, h => h.Subtitle == "V001");
        Assert.Equal("VENDOR", hit.Type);
        Assert.False(string.IsNullOrEmpty(hit.Id));
        Assert.False(string.IsNullOrEmpty(hit.Title)); // display name
    }

    [Fact]
    public async Task SearchAsync_CustomerHit_MapsTitleSubtitleType()
    {
        // Search by a customer name substring present in the mock ("Acme").
        var result = await CreateService().SearchAsync("Acme", 5, FinancialRole);

        Assert.NotEmpty(result.Customers);
        Assert.All(result.Customers, h =>
        {
            Assert.Equal("CUSTOMER", h.Type);
            Assert.False(string.IsNullOrEmpty(h.Id));
            Assert.False(string.IsNullOrEmpty(h.Title));    // display name
            Assert.False(string.IsNullOrEmpty(h.Subtitle)); // number
        });
    }
}
