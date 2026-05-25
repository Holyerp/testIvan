using Pinoles.Api.Infrastructure.BusinessCentral;

namespace Pinoles.Api.Application.Interfaces;

public interface IBcHttpClient
{
    Task<BcCollectionResponse<T>> GetCollectionAsync<T>(
        string entitySet,
        BcQueryOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync<T>(
        string entitySet,
        string id,
        BcQueryOptions? options = null,
        CancellationToken cancellationToken = default);
}
