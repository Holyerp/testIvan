using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Application.Purchase;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Application.Purchase;

public class PurchaseServiceTests
{
    private static PurchaseService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        return new PurchaseService(bc, new PurchaseInvoiceMapper());
    }

    [Fact]
    public async Task GetInvoicesAsync_OpenInvoices_ReturnsItems()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoices", 1, 20, null, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task GetInvoicesAsync_RespectsPageSize()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoices", 1, 3, null, null, null, null, null, null);
        Assert.True(result.Items.Count <= 3);
    }

    [Fact]
    public async Task GetInvoicesAsync_ReturnsTotalCount()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoices", 1, 3, null, null, null, null, null, null);
        Assert.True(result.Total >= result.Items.Count);
        Assert.True(result.Total > 3); // there are 10 mock open purchase invoices
    }

    [Fact]
    public async Task GetInvoicesAsync_NormalizesStatusToWireValue()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoices", 1, 50, null, null, null, null, null, null);
        // Every status must be one of the documented wire values.
        Assert.All(result.Items, i => Assert.Contains(i.Status, new[] { "OPEN", "PARTIAL", "PAID" }));
        Assert.Contains(result.Items, i => i.Status == "OPEN");
        Assert.Contains(result.Items, i => i.Status == "PAID");
        Assert.Contains(result.Items, i => i.Status == "PARTIAL");
    }

    [Fact]
    public async Task GetInvoicesAsync_CreditMemos_ReturnsItems()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseCreditMemos", 1, 20, null, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.StartsWith("PCM-", i.Number));
    }

    [Fact]
    public async Task GetInvoicesAsync_CreditMemos_UsesCreditMemoStatusValues()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseCreditMemos", 1, 50, null, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        // Credit memos only ever surface OPEN | POSTED (never PARTIAL/PAID).
        Assert.All(result.Items, i => Assert.Contains(i.Status, new[] { "OPEN", "POSTED" }));
    }

    [Fact]
    public async Task GetInvoicesAsync_PostedInvoices_ReturnsItems()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoicesPosted", 1, 20, null, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.StartsWith("PPI-", i.Number));
    }

    [Fact]
    public async Task GetInvoicesAsync_SearchNarrowsResults()
    {
        var service = CreateService();
        var all = await service.GetInvoicesAsync(
            "purchaseInvoices", 1, 50, null, null, null, null, null, null);
        var filtered = await service.GetInvoicesAsync(
            "purchaseInvoices", 1, 50, "Supplier A", null, null, null, null, null);

        Assert.True(filtered.Items.Count < all.Items.Count);
        Assert.NotEmpty(filtered.Items);
        Assert.All(filtered.Items, i => Assert.Contains("Supplier A", i.VendorName));
    }

    [Fact]
    public async Task GetInvoicesAsync_SearchByNumber_Matches()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoices", 1, 50, "PI-001", null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        Assert.Contains(result.Items, i => i.Number == "PI-001");
    }

    [Fact]
    public async Task GetInvoicesAsync_StatusFilterNarrowsToPaid()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoices", 1, 50, null, null, null, "Paid", null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.Equal("PAID", i.Status));
    }

    [Fact]
    public async Task GetInvoicesAsync_InvalidPage_DefaultsToOne()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoices", 0, 20, null, null, null, null, null, null);
        Assert.Equal(1, result.Page);
    }

    [Fact]
    public async Task GetInvoicesAsync_InvalidPageSize_DefaultsTo20()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoices", 1, 9999, null, null, null, null, null, null);
        Assert.Equal(20, result.PageSize);
    }

    [Fact]
    public async Task GetInvoicesAsync_MapsCoreFields()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseInvoices", 1, 20, null, null, null, null, null, null);
        var first = result.Items.First();
        Assert.False(string.IsNullOrEmpty(first.Number));
        Assert.False(string.IsNullOrEmpty(first.VendorName));
        Assert.False(string.IsNullOrEmpty(first.PostingDate));
        Assert.False(string.IsNullOrEmpty(first.DueDate));
    }

    [Fact]
    public async Task GetInvoicesAsync_UnknownEntitySet_ReturnsEmpty()
    {
        var result = await CreateService().GetInvoicesAsync(
            "doesNotExist", 1, 20, null, null, null, null, null, null);
        Assert.Empty(result.Items);
    }
}
