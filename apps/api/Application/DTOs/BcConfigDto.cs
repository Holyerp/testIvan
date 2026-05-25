namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// Non-sensitive Business Central connection configuration shown on the admin Settings
/// screen (US-022). SECURITY: this DTO deliberately omits the service-principal
/// credentials (<c>ClientId</c> / <c>ClientSecret</c> from <c>BcOptions</c>). Only the
/// tenant identity, company, environment, base URL and mock-mode flag are exposed so an
/// admin can read the connection target without leaking the secret. The credential fields
/// MUST NEVER be added here.
/// </summary>
public class BcConfigDto
{
    public string TenantId { get; set; } = string.Empty;   // masked identity — never the secret
    public string CompanyId { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty; // derived from BaseUrl; "Mock" in mock mode
    public string BaseUrl { get; set; } = string.Empty;
    public bool UseMock { get; set; }
    public List<EntitySyncStatusDto> LastSync { get; set; } = new();
}

/// <summary>Per-entity last-successful-sync timestamp shown in the Settings "Last Sync" table.</summary>
public class EntitySyncStatusDto
{
    public string EntityType { get; set; } = string.Empty;
    public string? LastSyncedAt { get; set; }   // ISO-8601 UTC, or null when never synced
}

/// <summary>
/// Result of an on-demand BC connectivity probe (US-022). The probe always returns HTTP 200;
/// success/failure is carried inside this payload (<c>Success</c>). On failure, <c>ErrorCode</c>
/// describes the integration problem (e.g. <c>INTEGRATION_BC_UNAVAILABLE</c>) and credentials
/// are never echoed.
/// </summary>
public class BcConnectionTestResultDto
{
    public bool Success { get; set; }
    public long DurationMs { get; set; }
    public string? LastSuccessfulSyncAt { get; set; }   // ISO-8601 UTC on success; null on failure
    public string? ErrorCode { get; set; }              // null on success
}
