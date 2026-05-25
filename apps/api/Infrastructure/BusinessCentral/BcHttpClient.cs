using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Infrastructure.BusinessCentral;

public class BcHttpClient : IBcHttpClient
{
    private readonly HttpClient _http;
    private readonly IBcAuthService _auth;
    private readonly BcOptions _options;
    private readonly ILogger<BcHttpClient> _logger;

    public BcHttpClient(
        HttpClient http,
        IBcAuthService auth,
        IOptions<BcOptions> options,
        ILogger<BcHttpClient> logger)
    {
        _http = http;
        _auth = auth;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<BcCollectionResponse<T>> GetCollectionAsync<T>(
        string entitySet,
        BcQueryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var token = await _auth.GetAccessTokenAsync(cancellationToken);
        var qs = options?.ToQueryString() ?? string.Empty;
        var url = $"{_options.BaseUrl}/v2.0/{_options.TenantId}/{_options.CompanyId}/api/v2.0/{entitySet}{qs}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Add("OData-MaxVersion", "4.0");

        _logger.LogDebug("BC GET {EntitySet} {QueryString}", entitySet, qs);
        var response = await _http.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<BcCollectionResponse<T>>(
                   content,
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
               ?? new BcCollectionResponse<T>();
    }

    public async Task<T?> GetByIdAsync<T>(
        string entitySet,
        string id,
        CancellationToken cancellationToken = default)
    {
        var token = await _auth.GetAccessTokenAsync(cancellationToken);
        var url = $"{_options.BaseUrl}/v2.0/{_options.TenantId}/{_options.CompanyId}/api/v2.0/{entitySet}({id})";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Add("OData-MaxVersion", "4.0");

        var response = await _http.SendAsync(request, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return default;
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
