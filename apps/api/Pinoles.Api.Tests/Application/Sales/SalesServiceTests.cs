using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Application.Sales;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Application.Sales;

public class SalesServiceTests
{
    private static SalesService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        return new SalesService(bc, new SalesInvoiceMapper());
    }

    [Fact]
    public async Task GetInvoicesAsync_OpenInvoices_ReturnsItems()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoices", 1, 20, null, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task GetInvoicesAsync_RespectsPageSize()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoices", 1, 3, null, null, null, null, null, null);
        Assert.True(result.Items.Count <= 3);
    }

    [Fact]
    public async Task GetInvoicesAsync_ReturnsTotalCount()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoices", 1, 3, null, null, null, null, null, null);
        Assert.True(result.Total >= result.Items.Count);
        Assert.True(result.Total > 3); // there are 12 mock open invoices
    }

    [Fact]
    public async Task GetInvoicesAsync_NormalizesStatusToWireValue()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoices", 1, 50, null, null, null, null, null, null);
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
            "salesCreditMemos", 1, 20, null, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.StartsWith("SCM-", i.Number));
    }

    [Fact]
    public async Task GetInvoicesAsync_PostedInvoices_ReturnsItems()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoicesPosted", 1, 20, null, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.StartsWith("PSI-", i.Number));
    }

    [Fact]
    public async Task GetInvoicesAsync_SearchNarrowsResults()
    {
        var service = CreateService();
        var all = await service.GetInvoicesAsync(
            "salesInvoices", 1, 50, null, null, null, null, null, null);
        var filtered = await service.GetInvoicesAsync(
            "salesInvoices", 1, 50, "Acme", null, null, null, null, null);

        Assert.True(filtered.Items.Count < all.Items.Count);
        Assert.NotEmpty(filtered.Items);
        Assert.All(filtered.Items, i => Assert.Contains("Acme", i.CustomerName));
    }

    [Fact]
    public async Task GetInvoicesAsync_SearchByNumber_Matches()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoices", 1, 50, "SI-001", null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        Assert.Contains(result.Items, i => i.Number == "SI-001");
    }

    [Fact]
    public async Task GetInvoicesAsync_StatusFilterNarrowsToPaid()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoices", 1, 50, null, null, null, "Paid", null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.Equal("PAID", i.Status));
    }

    [Fact]
    public async Task GetInvoicesAsync_InvalidPage_DefaultsToOne()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoices", 0, 20, null, null, null, null, null, null);
        Assert.Equal(1, result.Page);
    }

    [Fact]
    public async Task GetInvoicesAsync_InvalidPageSize_DefaultsTo20()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoices", 1, 9999, null, null, null, null, null, null);
        Assert.Equal(20, result.PageSize);
    }

    [Fact]
    public async Task GetInvoicesAsync_MapsCoreFields()
    {
        var result = await CreateService().GetInvoicesAsync(
            "salesInvoices", 1, 20, null, null, null, null, null, null);
        var first = result.Items.First();
        Assert.False(string.IsNullOrEmpty(first.Number));
        Assert.False(string.IsNullOrEmpty(first.CustomerName));
        Assert.False(string.IsNullOrEmpty(first.PostingDate));
        Assert.False(string.IsNullOrEmpty(first.DueDate));
    }
}
