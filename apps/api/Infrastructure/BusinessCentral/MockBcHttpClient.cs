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
            new() { ["id"] = "c001", ["number"] = "C001", ["displayName"] = "Acme d.o.o.",        ["city"] = "Beograd",   ["balance"] = 150000.00m, ["balanceDue"] = 45000.00m  },
            new() { ["id"] = "c002", ["number"] = "C002", ["displayName"] = "Delta Corp",         ["city"] = "Novi Sad",  ["balance"] = 280000.00m, ["balanceDue"] = 0m         },
            new() { ["id"] = "c003", ["number"] = "C003", ["displayName"] = "Sigma Trade",        ["city"] = "Niš",       ["balance"] = 95000.00m,  ["balanceDue"] = 95000.00m  },
            new() { ["id"] = "c004", ["number"] = "C004", ["displayName"] = "Omega Logistika",    ["city"] = "Beograd",   ["balance"] = 412000.00m, ["balanceDue"] = 120000.00m },
            new() { ["id"] = "c005", ["number"] = "C005", ["displayName"] = "Beta Gradnja d.o.o.", ["city"] = "Kragujevac", ["balance"] = 67000.00m, ["balanceDue"] = 0m         },
            new() { ["id"] = "c006", ["number"] = "C006", ["displayName"] = "Gama Petrol",        ["city"] = "Subotica",  ["balance"] = 530000.00m, ["balanceDue"] = 210000.00m },
            new() { ["id"] = "c007", ["number"] = "C007", ["displayName"] = "Lambda Tehnika",     ["city"] = "Novi Sad",  ["balance"] = 88000.00m,  ["balanceDue"] = 12000.00m  },
            new() { ["id"] = "c008", ["number"] = "C008", ["displayName"] = "Zenit Trgovina",     ["city"] = "Niš",       ["balance"] = 145000.00m, ["balanceDue"] = 0m         },
            new() { ["id"] = "c009", ["number"] = "C009", ["displayName"] = "Vega Mont d.o.o.",   ["city"] = "Čačak",     ["balance"] = 39000.00m,  ["balanceDue"] = 39000.00m  },
            new() { ["id"] = "c010", ["number"] = "C010", ["displayName"] = "Nova Elektro",       ["city"] = "Beograd",   ["balance"] = 274000.00m, ["balanceDue"] = 60000.00m  },
            new() { ["id"] = "c011", ["number"] = "C011", ["displayName"] = "Kappa Distribucija", ["city"] = "Zrenjanin", ["balance"] = 198000.00m, ["balanceDue"] = 0m         },
            new() { ["id"] = "c012", ["number"] = "C012", ["displayName"] = "Theta Komerc",       ["city"] = "Pančevo",   ["balance"] = 76000.00m,  ["balanceDue"] = 25000.00m  },
        };

        // Simple in-memory filter that mimics OData contains(displayName,'x') or contains(number,'x').
        // We extract the search term from the generated filter string rather than reimplementing OData parsing.
        var filtered = ApplyCustomerFilter(customers, options?.Filter);

        var top = options?.Top ?? filtered.Count;
        var skip = options?.Skip ?? 0;
        var paged = filtered.Skip(skip).Take(top).ToList();
        var json = JsonSerializer.Serialize(paged);
        var typed = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        return new BcCollectionResponse<T>
        {
            Value = typed,
            Count = options?.Count == true ? filtered.Count : null
        };
    }

    private static List<Dictionary<string, object>> ApplyCustomerFilter(
        List<Dictionary<string, object>> customers, string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter)) return customers;

        // Extract the term between the first pair of single quotes in the contains(...) expression.
        var firstQuote = filter.IndexOf('\'');
        if (firstQuote < 0) return customers;
        var secondQuote = filter.IndexOf('\'', firstQuote + 1);
        if (secondQuote < 0) return customers;

        var term = filter.Substring(firstQuote + 1, secondQuote - firstQuote - 1)
            .Replace("''", "'");
        if (string.IsNullOrWhiteSpace(term)) return customers;

        return customers.Where(c =>
            c["displayName"].ToString()!.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            c["number"].ToString()!.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private static BcCollectionResponse<T> CreateMockSalesInvoices<T>(BcQueryOptions? options)
    {
        var invoices = GetMockInvoiceData();
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

    // Invoices spread across the last 6 months so the dashboard trend chart always
    // has data within its rolling 6-month window. Dates are computed relative to now.
    private static List<Dictionary<string, object>> GetMockInvoiceData()
    {
        var now = DateTime.UtcNow;
        return new List<Dictionary<string, object>>
        {
            new() { ["id"] = "inv001", ["number"] = "SI-001", ["customerName"] = "Acme d.o.o.", ["postingDate"] = now.AddMonths(-5).ToString("yyyy-MM-dd"), ["totalAmountIncludingTax"] = 45000.00m,  ["status"] = "Open" },
            new() { ["id"] = "inv002", ["number"] = "SI-002", ["customerName"] = "Delta Corp",  ["postingDate"] = now.AddMonths(-4).ToString("yyyy-MM-dd"), ["totalAmountIncludingTax"] = 78000.00m,  ["status"] = "Open" },
            new() { ["id"] = "inv003", ["number"] = "SI-003", ["customerName"] = "Sigma Trade", ["postingDate"] = now.AddMonths(-3).ToString("yyyy-MM-dd"), ["totalAmountIncludingTax"] = 120000.00m, ["status"] = "Open" },
            new() { ["id"] = "inv004", ["number"] = "SI-004", ["customerName"] = "Acme d.o.o.", ["postingDate"] = now.AddMonths(-2).ToString("yyyy-MM-dd"), ["totalAmountIncludingTax"] = 56000.00m,  ["status"] = "Open" },
            new() { ["id"] = "inv005", ["number"] = "SI-005", ["customerName"] = "Delta Corp",  ["postingDate"] = now.AddMonths(-1).ToString("yyyy-MM-dd"), ["totalAmountIncludingTax"] = 99000.00m,  ["status"] = "Open" },
            new() { ["id"] = "inv006", ["number"] = "SI-006", ["customerName"] = "Sigma Trade", ["postingDate"] = now.AddDays(-3).ToString("yyyy-MM-dd"),    ["totalAmountIncludingTax"] = 33000.00m,  ["status"] = "Open" },
            new() { ["id"] = "inv007", ["number"] = "SI-007", ["customerName"] = "Acme d.o.o.", ["postingDate"] = now.AddDays(-40).ToString("yyyy-MM-dd"),   ["totalAmountIncludingTax"] = 67000.00m,  ["status"] = "Open" },
            new() { ["id"] = "inv008", ["number"] = "SI-008", ["customerName"] = "Delta Corp",  ["postingDate"] = now.AddDays(-10).ToString("yyyy-MM-dd"),   ["totalAmountIncludingTax"] = 88000.00m,  ["status"] = "Paid" },
        };
    }

    private static BcCollectionResponse<T> CreateMockVendors<T>(BcQueryOptions? options)
    {
        var vendors = new List<Dictionary<string, object>>
        {
            new() { ["id"] = "v001", ["number"] = "V001", ["displayName"] = "Supplier A d.o.o.", ["city"] = "Beograd",  ["balance"] = 55000.00m },
            new() { ["id"] = "v002", ["number"] = "V002", ["displayName"] = "Supplier B d.o.o.", ["city"] = "Novi Sad", ["balance"] = 32000.00m },
        };
        var top = options?.Top ?? vendors.Count;
        var skip = options?.Skip ?? 0;
        var paged = vendors.Skip(skip).Take(top).ToList();
        var json = JsonSerializer.Serialize(paged);
        var typed = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        return new BcCollectionResponse<T>
        {
            Value = typed,
            Count = options?.Count == true ? vendors.Count : null
        };
    }
}
