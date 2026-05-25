using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Application.Settings;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Pinoles.Api.Infrastructure.Caching;
using Xunit;

namespace Pinoles.Api.Tests.Application.Settings;

public class SettingsServiceTests
{
    private const string KnownSecret = "super-secret-client-secret-DO-NOT-LEAK";
    private const string KnownClientId = "11111111-2222-3333-4444-555555555555";

    private static BcOptions MockOptions() => new()
    {
        UseMock = true,
        TenantId = "tenant-abcdef-1234",
        ClientId = KnownClientId,
        ClientSecret = KnownSecret,
        BaseUrl = "https://api.businesscentral.dynamics.com/v2.0/production",
        CompanyId = "company-001",
        CacheSeconds = 300,
    };

    private static SettingsService CreateService(IBcHttpClient? bc = null, BcOptions? options = null)
    {
        bc ??= new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance);
        var cache = new MemoryCacheService(new MemoryCache(new MemoryCacheOptions()));
        var opts = Options.Create(options ?? MockOptions());
        return new SettingsService(opts, bc, cache, NullLogger<SettingsService>.Instance);
    }

    // A BC client stub that always throws, to exercise the connection-test failure path.
    private sealed class ThrowingBcHttpClient : IBcHttpClient
    {
        public Task<BcCollectionResponse<T>> GetCollectionAsync<T>(
            string entitySet, BcQueryOptions? options = null, CancellationToken cancellationToken = default)
            => throw new HttpRequestException("BC unreachable");

        public Task<T?> GetByIdAsync<T>(
            string entitySet, string id, BcQueryOptions? options = null, CancellationToken cancellationToken = default)
            => throw new HttpRequestException("BC unreachable");
    }

    [Fact]
    public async Task GetBcConfig_ReturnsNonSensitiveFields()
    {
        var config = await CreateService().GetBcConfigAsync();

        Assert.True(config.UseMock);
        Assert.Equal("company-001", config.CompanyId);
        Assert.Equal("Mock", config.Environment);
        Assert.False(string.IsNullOrEmpty(config.BaseUrl));
    }

    [Fact]
    public async Task GetBcConfig_DoesNotExposeClientSecretOrClientId()
    {
        var config = await CreateService().GetBcConfigAsync();

        // Serialize the entire DTO and assert neither credential value appears anywhere.
        var serialized = JsonSerializer.Serialize(config);
        Assert.DoesNotContain(KnownSecret, serialized);
        Assert.DoesNotContain(KnownClientId, serialized);
    }

    [Fact]
    public async Task GetBcConfig_MasksTenantIdSoFullValueIsNotEchoed()
    {
        var config = await CreateService().GetBcConfigAsync();

        // The full tenant id must not be echoed verbatim; the masked form keeps only a prefix.
        Assert.NotEqual("tenant-abcdef-1234", config.TenantId);
        Assert.StartsWith("tena", config.TenantId);
    }

    [Fact]
    public async Task GetBcConfig_ListsTrackedEntitiesInLastSync()
    {
        var config = await CreateService().GetBcConfigAsync();

        var entities = config.LastSync.Select(e => e.EntityType).ToList();
        Assert.Contains("customers", entities);
        Assert.Contains("salesInvoices", entities);
        Assert.Contains("vendors", entities);
        Assert.Contains("items", entities);
    }

    [Fact]
    public async Task TestConnection_InMockMode_Succeeds()
    {
        var result = await CreateService().TestConnectionAsync();

        Assert.True(result.Success);
        Assert.True(result.DurationMs >= 0);
        Assert.False(string.IsNullOrEmpty(result.LastSuccessfulSyncAt));
        Assert.Null(result.ErrorCode);
    }

    [Fact]
    public async Task TestConnection_RecordsLastSyncTimestampForGetBcConfig()
    {
        var service = CreateService();
        await service.TestConnectionAsync();
        var config = await service.GetBcConfigAsync();

        // After a successful probe, every tracked entity carries a non-null last-sync timestamp.
        Assert.All(config.LastSync, e => Assert.False(string.IsNullOrEmpty(e.LastSyncedAt)));
    }

    [Fact]
    public async Task GetBcConfig_BeforeAnyProbe_LastSyncIsNull()
    {
        var config = await CreateService().GetBcConfigAsync();

        // No probe has run yet → no synthesized timestamps; the table renders "Never".
        Assert.All(config.LastSync, e => Assert.Null(e.LastSyncedAt));
    }

    [Fact]
    public async Task TestConnection_WhenBcThrows_ReturnsFailureWithIntegrationCode()
    {
        var service = CreateService(bc: new ThrowingBcHttpClient());

        var result = await service.TestConnectionAsync();

        Assert.False(result.Success);
        Assert.Equal("INTEGRATION_BC_UNAVAILABLE", result.ErrorCode);
        Assert.Null(result.LastSuccessfulSyncAt);
        Assert.True(result.DurationMs >= 0);
    }

    [Fact]
    public async Task GetBcConfig_LiveMode_DerivesEnvironmentFromBaseUrl()
    {
        var liveOptions = MockOptions();
        liveOptions.UseMock = false;
        var service = CreateService(bc: new MockBcHttpClient(NullLogger<MockBcHttpClient>.Instance), options: liveOptions);

        var config = await service.GetBcConfigAsync();

        Assert.False(config.UseMock);
        Assert.Equal("production", config.Environment);
    }
}
