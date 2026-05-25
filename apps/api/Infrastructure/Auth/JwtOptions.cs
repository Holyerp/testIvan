namespace Pinoles.Api.Infrastructure.Auth;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "pinoles-api";
    public string Audience { get; set; } = "pinoles-web";
    public int AccessTokenExpiryHours { get; set; } = 8;
    public int RefreshTokenExpiryDays { get; set; } = 7;
}
