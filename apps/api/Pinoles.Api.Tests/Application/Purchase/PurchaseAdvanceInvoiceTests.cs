using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Application.Purchase;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Application.Purchase;

/// <summary>
/// Tests for US-015 purchase advance (proforma) invoices. The list reuses
/// <see cref="PurchaseService.GetInvoicesAsync"/> over the purchaseAdvanceInvoices
/// collection; the detail uses <see cref="PurchaseService.GetAdvanceInvoiceByIdAsync"/>
/// which adds the payment-tracking block. Implemented against the standard BC schema
/// (Q-003 pending). Mirrors the sales advance tests on the vendor side.
/// </summary>
public class PurchaseAdvanceInvoiceTests
{
    private static PurchaseService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        return new PurchaseService(
            bc,
            new PurchaseInvoiceMapper(),
            new PurchaseInvoiceDetailMapper(),
            new PurchaseAdvanceInvoiceDetailMapper());
    }

    // ----- List -----

    [Fact]
    public async Task GetInvoicesAsync_AdvanceInvoices_ReturnsItems()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseAdvanceInvoices", 1, 20, null, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.StartsWith("PA-", i.Number));
    }

    [Fact]
    public async Task GetInvoicesAsync_AdvanceInvoices_RespectsPageSize()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseAdvanceInvoices", 1, 3, null, null, null, null, null, null);
        Assert.True(result.Items.Count <= 3);
    }

    [Fact]
    public async Task GetInvoicesAsync_AdvanceInvoices_ReturnsTotalCount()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseAdvanceInvoices", 1, 3, null, null, null, null, null, null);
        Assert.True(result.Total >= result.Items.Count);
        Assert.True(result.Total > 3); // 8 mock purchase advance invoices
    }

    [Fact]
    public async Task GetInvoicesAsync_AdvanceInvoices_PopulatesVendorName()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseAdvanceInvoices", 1, 50, null, null, null, null, null, null);
        Assert.All(result.Items, i => Assert.False(string.IsNullOrEmpty(i.VendorName)));
    }

    [Fact]
    public async Task GetInvoicesAsync_AdvanceInvoices_NormalizesStatusToWireValue()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseAdvanceInvoices", 1, 50, null, null, null, null, null, null);
        Assert.All(result.Items, i => Assert.Contains(i.Status, new[] { "OPEN", "PARTIAL", "PAID" }));
        Assert.Contains(result.Items, i => i.Status == "OPEN");
        Assert.Contains(result.Items, i => i.Status == "PARTIAL");
        Assert.Contains(result.Items, i => i.Status == "PAID");
    }

    [Fact]
    public async Task GetInvoicesAsync_AdvanceInvoices_StatusFilterNarrowsToPaid()
    {
        var result = await CreateService().GetInvoicesAsync(
            "purchaseAdvanceInvoices", 1, 50, null, null, null, "Paid", null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.Equal("PAID", i.Status));
    }

    [Fact]
    public async Task GetInvoicesAsync_AdvanceInvoices_SearchNarrowsResults()
    {
        var service = CreateService();
        var all = await service.GetInvoicesAsync(
            "purchaseAdvanceInvoices", 1, 50, null, null, null, null, null, null);
        var filtered = await service.GetInvoicesAsync(
            "purchaseAdvanceInvoices", 1, 50, "Supplier A", null, null, null, null, null);

        Assert.True(filtered.Items.Count < all.Items.Count);
        Assert.NotEmpty(filtered.Items);
        Assert.All(filtered.Items, i => Assert.Contains("Supplier A", i.VendorName));
    }

    // ----- Detail + payment tracking -----

    [Fact]
    public async Task GetAdvanceInvoiceByIdAsync_KnownId_ReturnsDetailWithLines()
    {
        var detail = await CreateService().GetAdvanceInvoiceByIdAsync("pav001");
        Assert.NotNull(detail);
        Assert.Equal("PA-2026-001", detail!.Header.Number);
        Assert.False(string.IsNullOrEmpty(detail.Header.VendorName));
        Assert.NotEmpty(detail.Lines);
        Assert.All(detail.Lines, l => Assert.False(string.IsNullOrEmpty(l.Description)));
    }

    [Fact]
    public async Task GetAdvanceInvoiceByIdAsync_UnknownId_ReturnsNull()
    {
        var detail = await CreateService().GetAdvanceInvoiceByIdAsync("does-not-exist");
        Assert.Null(detail);
    }

    [Fact]
    public async Task GetAdvanceInvoiceByIdAsync_ComputesTotals()
    {
        var detail = await CreateService().GetAdvanceInvoiceByIdAsync("pav001");
        Assert.NotNull(detail);
        var totals = detail!.Totals;
        Assert.True(totals.Subtotal > 0);
        Assert.True(totals.VatAmount > 0);
        Assert.Equal(totals.Subtotal + totals.VatAmount, totals.Total);
    }

    [Fact]
    public async Task GetAdvanceInvoiceByIdAsync_PaymentTracking_AmountEqualsPaidPlusRemaining()
    {
        var detail = await CreateService().GetAdvanceInvoiceByIdAsync("pav002");
        Assert.NotNull(detail);
        var pt = detail!.PaymentTracking;
        Assert.Equal(pt.AmountPaid + pt.Remaining, pt.Amount);
    }

    [Fact]
    public async Task GetAdvanceInvoiceByIdAsync_PaidStatus_FullyPaid()
    {
        // pav003 is "Paid" — amount paid equals the document total, nothing remaining.
        var detail = await CreateService().GetAdvanceInvoiceByIdAsync("pav003");
        Assert.NotNull(detail);
        var pt = detail!.PaymentTracking;
        Assert.Equal(pt.Amount, pt.AmountPaid);
        Assert.Equal(0m, pt.Remaining);
        Assert.Equal("PAID", detail.Header.Status);
    }

    [Fact]
    public async Task GetAdvanceInvoiceByIdAsync_OpenStatus_NothingPaid()
    {
        // pav001 is "Open" — nothing paid, full amount remaining.
        var detail = await CreateService().GetAdvanceInvoiceByIdAsync("pav001");
        Assert.NotNull(detail);
        var pt = detail!.PaymentTracking;
        Assert.Equal(0m, pt.AmountPaid);
        Assert.Equal(pt.Amount, pt.Remaining);
        Assert.Equal("OPEN", detail.Header.Status);
    }

    [Fact]
    public async Task GetAdvanceInvoiceByIdAsync_PartialStatus_SomePaidSomeRemaining()
    {
        // pav002 is "Partially Paid" — a positive amount paid AND a positive remainder.
        var detail = await CreateService().GetAdvanceInvoiceByIdAsync("pav002");
        Assert.NotNull(detail);
        var pt = detail!.PaymentTracking;
        Assert.True(pt.AmountPaid > 0);
        Assert.True(pt.Remaining > 0);
        Assert.Equal("PARTIAL", detail.Header.Status);
    }

    [Fact]
    public async Task GetAdvanceInvoiceByIdAsync_PaymentAmountMatchesDocumentTotal()
    {
        var detail = await CreateService().GetAdvanceInvoiceByIdAsync("pav004");
        Assert.NotNull(detail);
        Assert.Equal(detail!.Totals.Total, detail.PaymentTracking.Amount);
    }

    [Fact]
    public async Task GetAdvanceInvoiceByIdAsync_NormalizesStatusToWireValue()
    {
        var detail = await CreateService().GetAdvanceInvoiceByIdAsync("pav005");
        Assert.NotNull(detail);
        Assert.Contains(detail!.Header.Status, new[] { "OPEN", "PARTIAL", "PAID" });
    }
}
