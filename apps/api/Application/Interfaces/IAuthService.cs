using Pinoles.Api.Application.Auth;

namespace Pinoles.Api.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<LoginResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
}
