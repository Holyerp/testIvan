using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardKpisDto> GetKpisAsync(CancellationToken cancellationToken = default);
}
