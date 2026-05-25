using Microsoft.Extensions.Caching.Memory;
using Pinoles.Api.Infrastructure.Caching;
using Xunit;

namespace Pinoles.Api.Tests.Infrastructure.Caching;

public class MemoryCacheServiceTests
{
    private readonly MemoryCacheService _cache;

    public MemoryCacheServiceTests()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cache = new MemoryCacheService(memoryCache);
    }

    [Fact]
    public async Task SetAsync_ThenGetAsync_ReturnsValue()
    {
        await _cache.SetAsync("test-key", "test-value");
        var result = await _cache.GetAsync<string>("test-key");

        Assert.Equal("test-value", result);
    }

    [Fact]
    public async Task GetAsync_MissingKey_ReturnsNull()
    {
        var result = await _cache.GetAsync<string>("nonexistent");

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_RemovesValue()
    {
        await _cache.SetAsync("remove-key", "value");
        await _cache.RemoveAsync("remove-key");
        var result = await _cache.GetAsync<string>("remove-key");

        Assert.Null(result);
    }

    [Fact]
    public async Task SetAsync_WithExpiry_ExpiresCorrectly()
    {
        await _cache.SetAsync("expiry-key", "value", TimeSpan.FromMilliseconds(50));
        await Task.Delay(150);
        var result = await _cache.GetAsync<string>("expiry-key");

        Assert.Null(result);
    }
}
