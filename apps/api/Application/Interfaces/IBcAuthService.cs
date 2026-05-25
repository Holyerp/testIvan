namespace Pinoles.Api.Application.Interfaces;

public interface IBcAuthService
{
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}
