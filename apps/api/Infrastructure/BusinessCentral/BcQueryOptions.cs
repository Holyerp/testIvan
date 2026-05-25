namespace Pinoles.Api.Infrastructure.BusinessCentral;

public class BcQueryOptions
{
    public string? Filter { get; set; }   // $filter
    public string? Select { get; set; }   // $select
    public string? OrderBy { get; set; }  // $orderby
    public int? Top { get; set; }         // $top
    public int? Skip { get; set; }        // $skip
    public bool Count { get; set; }       // $count=true
    public string? Expand { get; set; }   // $expand

    public string ToQueryString()
    {
        var parts = new List<string>();
        if (!string.IsNullOrEmpty(Filter))  parts.Add($"$filter={Uri.EscapeDataString(Filter)}");
        if (!string.IsNullOrEmpty(Select))  parts.Add($"$select={Select}");
        if (!string.IsNullOrEmpty(OrderBy)) parts.Add($"$orderby={OrderBy}");
        if (Top.HasValue)                   parts.Add($"$top={Top}");
        if (Skip.HasValue)                  parts.Add($"$skip={Skip}");
        if (Count)                          parts.Add("$count=true");
        if (!string.IsNullOrEmpty(Expand))  parts.Add($"$expand={Expand}");
        return parts.Count > 0 ? "?" + string.Join("&", parts) : string.Empty;
    }
}
