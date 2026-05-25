using System.Security.Claims;
using Pinoles.Api.Domain.Entities;

namespace Pinoles.Api.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateAccessToken(string token);
}
