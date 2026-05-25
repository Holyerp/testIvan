namespace Pinoles.Api.Infrastructure.BusinessCentral;

public class BcOptions
{
    public const string SectionName = "BC";
    public bool UseMock { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
    public string ResourceUrl { get; set; } = "https://api.businesscentral.dynamics.com";
    public int CacheSeconds { get; set; } = 300;
}
