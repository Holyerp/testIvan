using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

/// <summary>
/// Admin Settings service (US-022): exposes the non-sensitive Business Central connection
/// configuration and an on-demand connectivity probe. Implementations MUST NOT surface the
/// BC service-principal credentials (ClientId / ClientSecret).
/// </summary>
public interface ISettingsService
{
    /// <summary>Build the non-sensitive BC config view + per-entity last-sync timestamps.</summary>
    Task<BcConfigDto> GetBcConfigAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Perform a lightweight BC probe (fetch a tiny collection), measure round-trip time and
    /// record the success time. Returns success/failure inside the result — never throws for a
    /// reachable-but-failed BC; integration failures map to <c>INTEGRATION_BC_UNAVAILABLE</c>.
    /// </summary>
    Task<BcConnectionTestResultDto> TestConnectionAsync(CancellationToken cancellationToken = default);
}
