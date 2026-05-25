using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Infrastructure.BusinessCentral;

public class BcAuthService : IBcAuthService
{
    private readonly BcOptions _options;
    private readonly ILogger<BcAuthService> _logger;
    private string? _cachedToken;
    private DateTime _tokenExpiry = DateTime.MinValue;

    public BcAuthService(IOptions<BcOptions> options, ILogger<BcAuthService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (_cachedToken != null && DateTime.UtcNow < _tokenExpiry.AddMinutes(-5))
            return _cachedToken;

        var credential = new ClientSecretCredential(_options.TenantId, _options.ClientId, _options.ClientSecret);
        var tokenRequest = new TokenRequestContext(new[] { $"{_options.ResourceUrl}/.default" });
        var token = await credential.GetTokenAsync(tokenRequest, cancellationToken);
        _cachedToken = token.Token;
        _tokenExpiry = token.ExpiresOn.UtcDateTime;

        _logger.LogInformation("BC access token refreshed, expires {ExpiresAt}", _tokenExpiry);
        return _cachedToken;
    }
}
