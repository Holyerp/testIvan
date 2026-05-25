using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Application.CreditDocuments;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Application.CreditDocuments;

public class CreditDocumentServiceTests
{
    private static CreditDocumentService CreateService()
    {
        var bc = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        return new CreditDocumentService(
            bc,
            new CreditDocumentMapper(),
            new CreditDocumentDetailMapper());
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_ReturnsItemsAcrossAllTypes()
    {
        var result = await CreateService().GetCreditDocumentsAsync(
            1, 50, null, null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        // The unified list mixes all three document types.
        Assert.Contains(result.Items, i => i.Type == CreditDocumentType.CreditMemo);
        Assert.Contains(result.Items, i => i.Type == CreditDocumentType.DebitMemo);
        Assert.Contains(result.Items, i => i.Type == CreditDocumentType.Storno);
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_RespectsPageSize()
    {
        var result = await CreateService().GetCreditDocumentsAsync(
            1, 3, null, null, null, null, null, null);
        Assert.True(result.Items.Count <= 3);
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_ReturnsTotalCount()
    {
        var result = await CreateService().GetCreditDocumentsAsync(
            1, 3, null, null, null, null, null, null);
        Assert.True(result.Total >= result.Items.Count);
        Assert.True(result.Total > 3); // there are 12 mock credit documents
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_TypeFilterNarrowsToCreditMemo()
    {
        var result = await CreateService().GetCreditDocumentsAsync(
            1, 50, null, null, null, CreditDocumentType.CreditMemo, null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.Equal(CreditDocumentType.CreditMemo, i.Type));
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_TypeFilterNarrowsToStorno()
    {
        var result = await CreateService().GetCreditDocumentsAsync(
            1, 50, null, null, null, CreditDocumentType.Storno, null, null);
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, i => Assert.Equal(CreditDocumentType.Storno, i.Type));
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_TypeFilterNarrowsResults()
    {
        var service = CreateService();
        var all = await service.GetCreditDocumentsAsync(1, 50, null, null, null, null, null, null);
        var debits = await service.GetCreditDocumentsAsync(
            1, 50, null, null, null, CreditDocumentType.DebitMemo, null, null);

        Assert.True(debits.Items.Count < all.Items.Count);
        Assert.NotEmpty(debits.Items);
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_SearchNarrowsByNumber()
    {
        var result = await CreateService().GetCreditDocumentsAsync(
            1, 50, "CN-2026-001", null, null, null, null, null);
        Assert.NotEmpty(result.Items);
        Assert.Contains(result.Items, i => i.Number == "CN-2026-001");
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_SearchNarrowsByParty()
    {
        var service = CreateService();
        var all = await service.GetCreditDocumentsAsync(1, 50, null, null, null, null, null, null);
        var filtered = await service.GetCreditDocumentsAsync(1, 50, "Acme", null, null, null, null, null);

        Assert.True(filtered.Items.Count < all.Items.Count);
        Assert.NotEmpty(filtered.Items);
        Assert.All(filtered.Items, i => Assert.Contains("Acme", i.PartyName));
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_NormalizesStatusToWireValue()
    {
        var result = await CreateService().GetCreditDocumentsAsync(
            1, 50, null, null, null, null, null, null);
        // Correction documents only ever surface OPEN | POSTED.
        Assert.All(result.Items, i => Assert.Contains(i.Status, new[] { "OPEN", "POSTED" }));
        Assert.Contains(result.Items, i => i.Status == "OPEN");
        Assert.Contains(result.Items, i => i.Status == "POSTED");
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_NormalizesTypeToWireValue()
    {
        var result = await CreateService().GetCreditDocumentsAsync(
            1, 50, null, null, null, null, null, null);
        Assert.All(result.Items, i => Assert.Contains(i.Type, CreditDocumentType.All));
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_MapsOriginalInvoiceReference()
    {
        var result = await CreateService().GetCreditDocumentsAsync(
            1, 50, null, null, null, null, null, null);
        Assert.All(result.Items, i => Assert.False(string.IsNullOrEmpty(i.OriginalInvoiceNumber)));
    }

    [Fact]
    public async Task GetCreditDocumentsAsync_InvalidType_IgnoresFilter()
    {
        var service = CreateService();
        var all = await service.GetCreditDocumentsAsync(1, 50, null, null, null, null, null, null);
        // An unknown type is silently ignored (defends against injection); list unaffected.
        var bogus = await service.GetCreditDocumentsAsync(
            1, 50, null, null, null, "NOT_A_TYPE", null, null);
        Assert.Equal(all.Items.Count, bogus.Items.Count);
    }

    [Fact]
    public async Task GetCreditDocumentByIdAsync_KnownId_ReturnsDetailWithLines()
    {
        var detail = await CreateService().GetCreditDocumentByIdAsync("cd001");
        Assert.NotNull(detail);
        Assert.Equal("CN-2026-001", detail!.Header.Number);
        Assert.Equal(CreditDocumentType.CreditMemo, detail.Header.Type);
        Assert.False(string.IsNullOrEmpty(detail.Header.PartyName));
        Assert.False(string.IsNullOrEmpty(detail.Header.OriginalInvoiceNumber));
        Assert.NotEmpty(detail.Lines);
        Assert.All(detail.Lines, l => Assert.False(string.IsNullOrEmpty(l.Description)));
    }

    [Fact]
    public async Task GetCreditDocumentByIdAsync_StornoKnownId_ReturnsStornoType()
    {
        var detail = await CreateService().GetCreditDocumentByIdAsync("cd009");
        Assert.NotNull(detail);
        Assert.Equal(CreditDocumentType.Storno, detail!.Header.Type);
        Assert.Equal("SI-001", detail.Header.OriginalInvoiceNumber);
    }

    [Fact]
    public async Task GetCreditDocumentByIdAsync_UnknownId_ReturnsNull()
    {
        var detail = await CreateService().GetCreditDocumentByIdAsync("does-not-exist");
        Assert.Null(detail);
    }

    [Fact]
    public async Task GetCreditDocumentByIdAsync_ComputesTotals()
    {
        var detail = await CreateService().GetCreditDocumentByIdAsync("cd002");
        Assert.NotNull(detail);
        var totals = detail!.Totals;
        Assert.True(totals.Subtotal > 0);
        Assert.True(totals.VatAmount > 0);
        Assert.Equal(totals.Subtotal + totals.VatAmount, totals.Total);
    }

    [Fact]
    public async Task GetCreditDocumentByIdAsync_NormalizesStatusToWireValue()
    {
        var detail = await CreateService().GetCreditDocumentByIdAsync("cd001");
        Assert.NotNull(detail);
        Assert.Contains(detail!.Header.Status, new[] { "OPEN", "POSTED" });
    }
}
