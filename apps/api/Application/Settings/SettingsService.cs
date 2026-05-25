using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Settings;

/// <summary>
/// Admin Settings service (US-022). Reads the non-sensitive fields of <see cref="BcOptions"/>
/// and probes BC connectivity via <see cref="IBcHttpClient"/>. SECURITY: never reads or returns
/// <c>BcOptions.ClientId</c> / <c>BcOptions.ClientSecret</c> — the credentials stay in config and
/// in the auth client only. The cache holds per-entity last-successful-sync timestamps so the
/// Settings "Last Sync" table reflects real probe activity.
/// </summary>
public class SettingsService : ISettingsService
{
    private readonly BcOptions _bcOptions;
    private readonly IBcHttpClient _bc;
    private readonly ICacheService _cache;
    private readonly ILogger<SettingsService> _logger;

    // Entity types surfaced on the Settings "Last Sync" table. Stable order for the UI.
    private static readonly string[] TrackedEntities = { "customers", "salesInvoices", "vendors", "items" };
    private const string LastSyncCachePrefix = "settings:lastSync:";

    public SettingsService(
        IOptions<BcOptions> bcOptions,
        IBcHttpClient bc,
        ICacheService cache,
        ILogger<SettingsService> logger)
    {
        _bcOptions = bcOptions.Value;
        _bc = bc;
        _cache = cache;
        _logger = logger;
    }

    public async Task<BcConfigDto> GetBcConfigAsync(CancellationToken cancellationToken = default)
    {
        var lastSync = new List<EntitySyncStatusDto>();
        foreach (var entity in TrackedEntities)
        {
            var stamp = await _cache.GetAsync<string>(LastSyncCachePrefix + entity, cancellationToken);
            lastSync.Add(new EntitySyncStatusDto { EntityType = entity, LastSyncedAt = stamp });
        }

        return new BcConfigDto
        {
            TenantId = MaskTenantId(_bcOptions.TenantId),
            CompanyId = _bcOptions.CompanyId,
            Environment = DeriveEnvironment(_bcOptions),
            BaseUrl = _bcOptions.BaseUrl,
            UseMock = _bcOptions.UseMock,
            LastSync = lastSync,
        };
    }

    public async Task<BcConnectionTestResultDto> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            // Lightweight probe: ask BC for a single customer record (Top=1). Cheapest read that
            // still exercises auth + transport. In mock mode this returns immediately.
            await _bc.GetCollectionAsync<BcCustomer>(
                "customers", new BcQueryOptions { Top = 1 }, cancellationToken);
            stopwatch.Stop();

            var now = DateTime.UtcNow;
            var nowIso = now.ToString("O");
            // Record the success time against every tracked entity so the "Last Sync" table
            // reflects this probe on the next config read.
            foreach (var entity in TrackedEntities)
                await _cache.SetAsync(LastSyncCachePrefix + entity, nowIso, TimeSpan.FromDays(7), cancellationToken);

            return new BcConnectionTestResultDto
            {
                Success = true,
                DurationMs = stopwatch.ElapsedMilliseconds,
                LastSuccessfulSyncAt = nowIso,
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            // Log the failure WITHOUT any credential — only the failure fact + timing.
            _logger.LogWarning(ex, "BC connection test failed after {DurationMs}ms", stopwatch.ElapsedMilliseconds);
            return new BcConnectionTestResultDto
            {
                Success = false,
                DurationMs = stopwatch.ElapsedMilliseconds,
                ErrorCode = "INTEGRATION_BC_UNAVAILABLE",
            };
        }
    }

    /// <summary>
    /// Derive a human-readable environment name from the BC config. Mock mode is always "Mock".
    /// Otherwise the last non-empty path segment of the base URL (typically the BC environment
    /// name, e.g. "production" / "sandbox") is used, falling back to "Live" when unavailable.
    /// </summary>
    private static string DeriveEnvironment(BcOptions options)
    {
        if (options.UseMock) return "Mock";
        if (string.IsNullOrWhiteSpace(options.BaseUrl)) return "Live";

        var segment = options.BaseUrl
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault();
        return string.IsNullOrWhiteSpace(segment) ? "Live" : segment;
    }

    /// <summary>
    /// Mask the tenant id so the full GUID is not echoed verbatim: keep the first 4 chars and
    /// append an ellipsis. An empty tenant id stays empty. Never returns the client secret.
    /// </summary>
    private static string MaskTenantId(string tenantId)
    {
        if (string.IsNullOrWhiteSpace(tenantId)) return string.Empty;
        return tenantId.Length <= 4 ? tenantId : tenantId.Substring(0, 4) + "…";
    }
}
