using System.Text.Json;
using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Infrastructure.BusinessCentral;

public class MockBcHttpClient : IBcHttpClient
{
    private readonly ILogger<MockBcHttpClient> _logger;

    public MockBcHttpClient(ILogger<MockBcHttpClient> logger)
    {
        _logger = logger;
    }

    public Task<BcCollectionResponse<T>> GetCollectionAsync<T>(
        string entitySet,
        BcQueryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("MOCK BC GET {EntitySet}", entitySet);
        var result = entitySet switch
        {
            "customers"     => CreateMockCustomers<T>(options),
            "salesInvoices" => CreateMockSalesInvoices<T>(options),
            "vendors"       => CreateMockVendors<T>(options),
            _               => new BcCollectionResponse<T>()
        };
        return Task.FromResult(result);
    }

    public Task<T?> GetByIdAsync<T>(
        string entitySet,
        string id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("MOCK BC GET {EntitySet}({Id})", entitySet, id);
        return Task.FromResult<T?>(default);
    }

    private static BcCollectionResponse<T> CreateMockCustomers<T>(BcQueryOptions? options)
    {
        var customers = new List<Dictionary<string, object>>
        {
            new() { ["id"] = "c001", ["number"] = "C001", ["displayName"] = "Acme d.o.o.",   ["city"] = "Beograd",  ["balance"] = 150000.00m, ["balanceDue"] = 45000.00m },
            new() { ["id"] = "c002", ["number"] = "C002", ["displayName"] = "Delta Corp",    ["city"] = "Novi Sad", ["balance"] = 280000.00m, ["balanceDue"] = 0m        },
            new() { ["id"] = "c003", ["number"] = "C003", ["displayName"] = "Sigma Trade",   ["city"] = "Niš",      ["balance"] = 95000.00m,  ["balanceDue"] = 95000.00m },
        };
        var top = options?.Top ?? customers.Count;
        var skip = options?.Skip ?? 0;
        var paged = customers.Skip(skip).Take(top).ToList();
        var json = JsonSerializer.Serialize(paged);
        var typed = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        return new BcCollectionResponse<T>
        {
            Value = typed,
            Count = options?.Count == true ? customers.Count : null
        };
    }

    private static BcCollectionResponse<T> CreateMockSalesInvoices<T>(BcQueryOptions? options)
    {
        var invoices = new List<Dictionary<string, object>>
        {
            new() { ["id"] = "inv001", ["number"] = "SI-2026-001", ["customerName"] = "Acme d.o.o.", ["postingDate"] = "2026-05-01", ["totalAmountIncludingTax"] = 60000.00m,  ["status"] = "Open" },
            new() { ["id"] = "inv002", ["number"] = "SI-2026-002", ["customerName"] = "Delta Corp",  ["postingDate"] = "2026-05-10", ["totalAmountIncludingTax"] = 120000.00m, ["status"] = "Paid" },
        };
        var top = options?.Top ?? invoices.Count;
        var skip = options?.Skip ?? 0;
        var paged = invoices.Skip(skip).Take(top).ToList();
        var json = JsonSerializer.Serialize(paged);
        var typed = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        return new BcCollectionResponse<T>
        {
            Value = typed,
            Count = options?.Count == true ? invoices.Count : null
        };
    }

    private static BcCollectionResponse<T> CreateMockVendors<T>(BcQueryOptions? options)
    {
        var vendors = new List<Dictionary<string, object>>
        {
            new() { ["id"] = "v001", ["number"] = "V001", ["displayName"] = "Supplier A d.o.o.", ["city"] = "Beograd", ["balance"] = 55000.00m },
        };
        var json = JsonSerializer.Serialize(vendors);
        var typed = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        return new BcCollectionResponse<T> { Value = typed };
    }
}
