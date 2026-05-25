using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Xunit;

namespace Pinoles.Api.Tests.Infrastructure.BusinessCentral;

public class MockBcHttpClientTests
{
    private readonly MockBcHttpClient _client;

    public MockBcHttpClientTests()
    {
        _client = new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
    }

    [Fact]
    public async Task GetCollectionAsync_Customers_ReturnsItems()
    {
        var result = await _client.GetCollectionAsync<Dictionary<string, JsonElement>>("customers");

        Assert.NotNull(result);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task GetCollectionAsync_Customers_WithPagination_ReturnsPagedItems()
    {
        var options = new BcQueryOptions { Top = 2, Skip = 0 };
        var result = await _client.GetCollectionAsync<Dictionary<string, JsonElement>>("customers", options);

        Assert.Equal(2, result.Value.Count);
    }

    [Fact]
    public async Task GetCollectionAsync_UnknownEntitySet_ReturnsEmpty()
    {
        var result = await _client.GetCollectionAsync<Dictionary<string, JsonElement>>("unknownEntity");

        Assert.NotNull(result);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_KnownCustomerId_ReturnsCustomer()
    {
        var result = await _client.GetByIdAsync<Dictionary<string, JsonElement>>("customers", "c001");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownCustomerId_ReturnsNull()
    {
        var result = await _client.GetByIdAsync<Dictionary<string, JsonElement>>("customers", "does-not-exist");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownEntitySet_ReturnsNull()
    {
        var result = await _client.GetByIdAsync<Dictionary<string, JsonElement>>("unknownEntity", "c001");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCollectionAsync_WithCount_ReturnsCountValue()
    {
        var options = new BcQueryOptions { Count = true };
        var result = await _client.GetCollectionAsync<Dictionary<string, JsonElement>>("customers", options);

        Assert.NotNull(result.Count);
        Assert.True(result.Count > 0);
    }

    [Fact]
    public async Task GetCollectionAsync_SalesInvoices_ReturnsItems()
    {
        var result = await _client.GetCollectionAsync<Dictionary<string, JsonElement>>("salesInvoices");

        Assert.NotNull(result);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task GetCollectionAsync_Vendors_ReturnsItems()
    {
        var result = await _client.GetCollectionAsync<Dictionary<string, JsonElement>>("vendors");

        Assert.NotNull(result);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_KnownSalesInvoiceId_ReturnsInvoiceWithLines()
    {
        var result = await _client.GetByIdAsync<Dictionary<string, JsonElement>>("salesInvoices", "inv001");

        Assert.NotNull(result);
        Assert.True(result!.ContainsKey("salesInvoiceLines"));
        Assert.NotEqual(JsonValueKind.Null, result["salesInvoiceLines"].ValueKind);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownSalesInvoiceId_ReturnsNull()
    {
        var result = await _client.GetByIdAsync<Dictionary<string, JsonElement>>("salesInvoices", "does-not-exist");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_KnownPostedInvoiceId_ReturnsInvoice()
    {
        var result = await _client.GetByIdAsync<Dictionary<string, JsonElement>>("salesInvoicesPosted", "psi001");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithExpandOption_StillReturnsInvoice()
    {
        var options = new BcQueryOptions { Expand = "salesInvoiceLines" };
        var result = await _client.GetByIdAsync<Dictionary<string, JsonElement>>("salesInvoices", "inv001", options);

        Assert.NotNull(result);
    }
}
