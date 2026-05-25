using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Interfaces;

public interface ICustomerService
{
    Task<PagedResultDto<CustomerListItemDto>> GetCustomersAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        CancellationToken cancellationToken = default);

    Task<CustomerDetailDto?> GetCustomerByIdAsync(
        string id,
        CancellationToken cancellationToken = default);
}
