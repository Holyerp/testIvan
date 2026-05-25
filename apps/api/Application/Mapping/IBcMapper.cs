namespace Pinoles.Api.Application.Mapping;

/// <summary>Maps a raw BC OData entity (TSource) to a Pinoles UI DTO (TTarget).</summary>
public interface IBcMapper<TSource, TTarget>
{
    TTarget Map(TSource source);
}
