using System.Text.Json.Serialization;

namespace Pinoles.Api.Infrastructure.BusinessCentral;

public class BcCollectionResponse<T>
{
    [JsonPropertyName("value")]
    public List<T> Value { get; set; } = new();

    [JsonPropertyName("@odata.count")]
    public int? Count { get; set; }

    [JsonPropertyName("@odata.nextLink")]
    public string? NextLink { get; set; }
}
