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
            "customers"           => CreateMockCustomers<T>(options),
            "salesInvoices"       => CreateMockInvoiceCollection<T>(GetMockInvoiceData(), options),
            "salesInvoicesPosted" => CreateMockInvoiceCollection<T>(GetMockPostedInvoiceData(), options),
            "salesCreditMemos"    => CreateMockInvoiceCollection<T>(GetMockCreditMemoData(), options),
            "salesCreditMemosPosted" => CreateMockInvoiceCollection<T>(GetMockPostedCreditMemoData(), options),
            "purchaseInvoices"        => CreateMockDocumentCollection<T>(GetMockPurchaseInvoiceData(), options, "vendorName"),
            "purchaseInvoicesPosted"  => CreateMockDocumentCollection<T>(GetMockPostedPurchaseInvoiceData(), options, "vendorName"),
            "purchaseCreditMemos"     => CreateMockDocumentCollection<T>(GetMockPurchaseCreditMemoData(), options, "vendorName"),
            "vendors"             => CreateMockVendors<T>(options),
            _                     => new BcCollectionResponse<T>()
        };
        return Task.FromResult(result);
    }

    // Mock ignores the actual $expand string in `options` but always populates line
    // items for the sales-invoice family, mimicking a BC GET with $expand=salesInvoiceLines.
    public Task<T?> GetByIdAsync<T>(
        string entitySet,
        string id,
        BcQueryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("MOCK BC GET {EntitySet}({Id})", entitySet, id);

        var match = entitySet switch
        {
            "customers"           => GetMockCustomerData().FirstOrDefault(c => (string)c["id"] == id),
            "salesInvoices"       => FindInvoiceWithLines(GetMockInvoiceData(), id),
            "salesInvoicesPosted" => FindInvoiceWithLines(GetMockPostedInvoiceData(), id),
            "salesCreditMemos"    => FindInvoiceWithLines(GetMockCreditMemoData(), id),
            "salesCreditMemosPosted" => FindInvoiceWithLines(GetMockPostedCreditMemoData(), id),
            _                     => null,
        };

        if (match == null) return Task.FromResult<T?>(default);

        var json = JsonSerializer.Serialize(match);
        var typed = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return Task.FromResult(typed);
    }

    // Find an invoice in the given list-view source by id and attach mock line items
    // so the detail mapper can compute non-trivial totals. Returns null for unknown ids.
    private static Dictionary<string, object>? FindInvoiceWithLines(
        List<Dictionary<string, object>> source, string id)
    {
        var match = source.FirstOrDefault(i => (string)i["id"] == id);
        if (match == null) return null;

        // Clone so the shared mock list-view source is never mutated across calls.
        var record = new Dictionary<string, object>(match)
        {
            ["billToAddress"] = $"{match["customerName"]}, Bulevar Oslobođenja 12, Beograd",
            ["paymentTerms"] = "30 dana",
            ["salesInvoiceLines"] = GetMockInvoiceLines(id),
        };
        return record;
    }

    // 2–4 realistic line items per invoice; VAT 20%. The set varies by id so different
    // invoices produce different (non-trivial) totals.
    private static List<Dictionary<string, object>> GetMockInvoiceLines(string id)
    {
        Dictionary<string, object> Line(string description, decimal qty, decimal unitPrice, decimal vat) => new()
        {
            ["description"] = description,
            ["quantity"] = qty,
            ["unitPrice"] = unitPrice,
            ["vatPercent"] = vat,
            ["lineAmount"] = qty * unitPrice,
        };

        return new List<Dictionary<string, object>>
        {
            Line("Konsultantske usluge", 10m, 5000.00m, 20m),
            Line("Licenca softvera (godišnja)", 1m, 18000.00m, 20m),
            Line("Implementacija i podešavanje", 4m, 3500.00m, 20m),
        };
    }

    // Single source of truth for mock customer records — used by both
    // GetCollectionAsync (customers branch) and GetByIdAsync.
    private static List<Dictionary<string, object>> GetMockCustomerData()
    {
        return new List<Dictionary<string, object>>
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
    }

    private static BcCollectionResponse<T> CreateMockCustomers<T>(BcQueryOptions? options)
    {
        var customers = GetMockCustomerData();

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

    // Shared collection builder for the sales-invoice family (open / posted / credit memos).
    // Applies the in-memory filter, then Skip/Top, and echoes @odata.count when requested.
    private static BcCollectionResponse<T> CreateMockInvoiceCollection<T>(
        List<Dictionary<string, object>> source, BcQueryOptions? options)
        => CreateMockDocumentCollection<T>(source, options, "customerName");

    // Generalized collection builder for any invoice-shaped document family. The
    // <paramref name="nameField"/> selects which party field (customerName / vendorName)
    // the contains-search and eq-match predicates apply to, so the sales and purchase
    // families share one filter + paging implementation (DRY).
    private static BcCollectionResponse<T> CreateMockDocumentCollection<T>(
        List<Dictionary<string, object>> source, BcQueryOptions? options, string nameField)
    {
        var documents = ApplyDocumentFilter(source, options?.Filter, nameField);
        var top = options?.Top ?? documents.Count;
        var skip = options?.Skip ?? 0;
        var paged = documents.Skip(skip).Take(top).ToList();
        var json = JsonSerializer.Serialize(paged);
        var typed = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        return new BcCollectionResponse<T>
        {
            Value = typed,
            Count = options?.Count == true ? documents.Count : null
        };
    }

    // Lightweight in-memory approximation of the OData filters this mock has to honor:
    //  - <nameField> eq 'X'                                  (detail / party filter)
    //  - contains(number,'t') or contains(<nameField>,'t')   (list search)
    //  - status eq 'Open'                                    (status filter)
    //  - postingDate ge '...' / postingDate le '...'         (date range)
    // We inspect which predicates are present rather than parsing OData fully.
    // <paramref name="nameField"/> is "customerName" for sales, "vendorName" for purchase.
    private static List<Dictionary<string, object>> ApplyDocumentFilter(
        List<Dictionary<string, object>> documents, string? filter, string nameField)
    {
        if (string.IsNullOrWhiteSpace(filter)) return documents;

        var result = documents;

        // <nameField> eq 'X' — exact match (party filter). Only when there's no contains().
        if (filter.Contains($"{nameField} eq '") && !filter.Contains("contains("))
        {
            var name = ExtractQuoted(filter, $"{nameField} eq '");
            if (name != null)
                result = result.Where(i =>
                    string.Equals(i[nameField].ToString(), name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // contains(...) search term — match number OR <nameField>.
        if (filter.Contains("contains("))
        {
            var term = ExtractQuoted(filter, "contains(");
            if (!string.IsNullOrWhiteSpace(term))
                result = result.Where(i =>
                    i["number"].ToString()!.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    i[nameField].ToString()!.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // status eq 'Open' — exact match on the BC-style status string.
        if (filter.Contains("status eq '"))
        {
            var status = ExtractQuoted(filter, "status eq '");
            if (status != null)
                result = result.Where(i =>
                    string.Equals(i["status"].ToString(), status, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // postingDate ge '...' / le '...' — lexical compare works for ISO yyyy-MM-dd.
        if (filter.Contains("postingDate ge '"))
        {
            var from = ExtractQuoted(filter, "postingDate ge '");
            if (from != null)
                result = result.Where(i =>
                    string.CompareOrdinal(i["postingDate"].ToString(), from) >= 0).ToList();
        }
        if (filter.Contains("postingDate le '"))
        {
            var to = ExtractQuoted(filter, "postingDate le '");
            if (to != null)
                result = result.Where(i =>
                    string.CompareOrdinal(i["postingDate"].ToString(), to) <= 0).ToList();
        }

        return result;
    }

    // Extract the first single-quoted literal that appears after the given marker.
    private static string? ExtractQuoted(string filter, string marker)
    {
        var markerIdx = filter.IndexOf(marker, StringComparison.Ordinal);
        if (markerIdx < 0) return null;
        var open = filter.IndexOf('\'', markerIdx);
        if (open < 0) return null;
        var close = filter.IndexOf('\'', open + 1);
        if (close < 0) return null;
        return filter.Substring(open + 1, close - open - 1).Replace("''", "'");
    }

    private static string Iso(DateTime d) => d.ToString("yyyy-MM-dd");

    // Open sales invoices spread across the last 6 months so the dashboard trend chart
    // always has data within its rolling 6-month window. Dates are relative to now.
    // dueDate = postingDate + 30 days; some records are intentionally overdue (due in the
    // past while status is not Paid). Status uses BC-style casing ("Open" / "Partially
    // Paid" / "Paid"); the SalesInvoiceMapper normalizes to the OPEN/PARTIAL/PAID wire value.
    private static List<Dictionary<string, object>> GetMockInvoiceData()
    {
        var now = DateTime.UtcNow;
        Dictionary<string, object> Inv(string id, string number, string customer, DateTime posting, decimal amount, string status) => new()
        {
            ["id"] = id,
            ["number"] = number,
            ["customerName"] = customer,
            ["postingDate"] = Iso(posting),
            ["dueDate"] = Iso(posting.AddDays(30)),
            ["totalAmountIncludingTax"] = amount,
            ["status"] = status,
        };

        return new List<Dictionary<string, object>>
        {
            Inv("inv001", "SI-001", "Acme d.o.o.", now.AddMonths(-5),  45000.00m,  "Open"),
            Inv("inv002", "SI-002", "Delta Corp",  now.AddMonths(-4),  78000.00m,  "Open"),
            Inv("inv003", "SI-003", "Sigma Trade", now.AddMonths(-3),  120000.00m, "Partially Paid"),
            Inv("inv004", "SI-004", "Acme d.o.o.", now.AddMonths(-2),  56000.00m,  "Open"),
            Inv("inv005", "SI-005", "Delta Corp",  now.AddMonths(-1),  99000.00m,  "Open"),
            Inv("inv006", "SI-006", "Sigma Trade", now.AddDays(-3),    33000.00m,  "Open"),
            Inv("inv007", "SI-007", "Acme d.o.o.", now.AddDays(-40),   67000.00m,  "Open"),           // overdue (due ~10 days ago, not paid)
            Inv("inv008", "SI-008", "Delta Corp",  now.AddDays(-10),   88000.00m,  "Paid"),
            Inv("inv009", "SI-009", "Sigma Trade", now.AddDays(-50),   42000.00m,  "Partially Paid"),  // overdue
            Inv("inv010", "SI-010", "Acme d.o.o.", now.AddDays(-65),   71000.00m,  "Open"),            // overdue
            Inv("inv011", "SI-011", "Delta Corp",  now.AddDays(-2),    18500.00m,  "Open"),
            Inv("inv012", "SI-012", "Sigma Trade", now.AddDays(-90),   135000.00m, "Paid"),
        };
    }

    // Posted sales invoices — already booked; mostly Paid with a few still Open.
    private static List<Dictionary<string, object>> GetMockPostedInvoiceData()
    {
        var now = DateTime.UtcNow;
        Dictionary<string, object> Inv(string id, string number, string customer, DateTime posting, decimal amount, string status) => new()
        {
            ["id"] = id,
            ["number"] = number,
            ["customerName"] = customer,
            ["postingDate"] = Iso(posting),
            ["dueDate"] = Iso(posting.AddDays(30)),
            ["totalAmountIncludingTax"] = amount,
            ["status"] = status,
        };

        return new List<Dictionary<string, object>>
        {
            Inv("psi001", "PSI-001", "Acme d.o.o.", now.AddMonths(-3), 64000.00m,  "Paid"),
            Inv("psi002", "PSI-002", "Delta Corp",  now.AddMonths(-3), 52000.00m,  "Paid"),
            Inv("psi003", "PSI-003", "Sigma Trade", now.AddMonths(-2), 89000.00m,  "Paid"),
            Inv("psi004", "PSI-004", "Omega Logistika", now.AddMonths(-2), 110000.00m, "Open"),
            Inv("psi005", "PSI-005", "Gama Petrol", now.AddMonths(-1), 230000.00m, "Paid"),
            Inv("psi006", "PSI-006", "Acme d.o.o.", now.AddDays(-20),  47500.00m,  "Open"),
            Inv("psi007", "PSI-007", "Delta Corp",  now.AddDays(-15),  61000.00m,  "Paid"),
            Inv("psi008", "PSI-008", "Sigma Trade", now.AddDays(-7),   28000.00m,  "Paid"),
        };
    }

    // Open (draft / unposted) sales credit memos. Credit memos use BC-style status
    // "Open" / "Posted"; the credit-memo normalizer maps these to the OPEN / POSTED
    // wire value (not the OPEN/PARTIAL/PAID invoice lifecycle).
    private static List<Dictionary<string, object>> GetMockCreditMemoData()
    {
        var now = DateTime.UtcNow;
        return new List<Dictionary<string, object>>
        {
            CreditMemo("scm001", "SCM-001", "Acme d.o.o.",     now.AddMonths(-2), 12000.00m, "Open"),
            CreditMemo("scm002", "SCM-002", "Delta Corp",      now.AddMonths(-1), 8500.00m,  "Open"),
            CreditMemo("scm003", "SCM-003", "Sigma Trade",     now.AddDays(-25),  15000.00m, "Open"),
            CreditMemo("scm004", "SCM-004", "Omega Logistika", now.AddDays(-12),  6200.00m,  "Open"),
            CreditMemo("scm005", "SCM-005", "Gama Petrol",     now.AddDays(-5),   9800.00m,  "Open"),
        };
    }

    // Posted sales credit memos — already booked. All status "Posted".
    private static List<Dictionary<string, object>> GetMockPostedCreditMemoData()
    {
        var now = DateTime.UtcNow;
        return new List<Dictionary<string, object>>
        {
            CreditMemo("pscm001", "PSCM-001", "Acme d.o.o.",  now.AddMonths(-3), 7400.00m,  "Posted"),
            CreditMemo("pscm002", "PSCM-002", "Delta Corp",   now.AddMonths(-2), 5200.00m,  "Posted"),
            CreditMemo("pscm003", "PSCM-003", "Sigma Trade",  now.AddMonths(-1), 11300.00m, "Posted"),
            CreditMemo("pscm004", "PSCM-004", "Lambda Tehnika", now.AddDays(-9), 3900.00m,  "Posted"),
        };
    }

    private static Dictionary<string, object> CreditMemo(
        string id, string number, string customer, DateTime posting, decimal amount, string status) => new()
    {
        ["id"] = id,
        ["number"] = number,
        ["customerName"] = customer,
        ["postingDate"] = Iso(posting),
        ["dueDate"] = Iso(posting.AddDays(30)),
        ["totalAmountIncludingTax"] = amount,
        ["status"] = status,
    };

    // ----- Purchase documents (US-009) -----
    // Mirror the sales mock but carry a vendorName instead of customerName. dueDate =
    // postingDate + 30 days; some records are intentionally overdue. Status uses BC-style
    // casing ("Open" / "Partially Paid" / "Paid"); the PurchaseInvoiceMapper normalizes
    // to the OPEN/PARTIAL/PAID wire value.
    private static Dictionary<string, object> Purchase(
        string id, string number, string vendor, DateTime posting, decimal amount, string status) => new()
    {
        ["id"] = id,
        ["number"] = number,
        ["vendorName"] = vendor,
        ["postingDate"] = Iso(posting),
        ["dueDate"] = Iso(posting.AddDays(30)),
        ["totalAmountIncludingTax"] = amount,
        ["status"] = status,
    };

    // Open purchase invoices — spread over the last 6 months; a few are overdue.
    private static List<Dictionary<string, object>> GetMockPurchaseInvoiceData()
    {
        var now = DateTime.UtcNow;
        return new List<Dictionary<string, object>>
        {
            Purchase("pinv001", "PI-001", "Supplier A d.o.o.",   now.AddMonths(-5), 38000.00m,  "Open"),
            Purchase("pinv002", "PI-002", "Supplier B d.o.o.",   now.AddMonths(-4), 64000.00m,  "Open"),
            Purchase("pinv003", "PI-003", "Materijal Promet",    now.AddMonths(-3), 112000.00m, "Partially Paid"),
            Purchase("pinv004", "PI-004", "Energo Snabdevanje",  now.AddMonths(-2), 87000.00m,  "Open"),
            Purchase("pinv005", "PI-005", "Supplier A d.o.o.",   now.AddMonths(-1), 41500.00m,  "Open"),
            Purchase("pinv006", "PI-006", "Tehno Oprema d.o.o.", now.AddDays(-4),   29000.00m,  "Open"),
            Purchase("pinv007", "PI-007", "Supplier B d.o.o.",   now.AddDays(-45),  73000.00m,  "Open"),            // overdue
            Purchase("pinv008", "PI-008", "Materijal Promet",    now.AddDays(-12),  95000.00m,  "Paid"),
            Purchase("pinv009", "PI-009", "Energo Snabdevanje",  now.AddDays(-55),  52000.00m,  "Partially Paid"),  // overdue
            Purchase("pinv010", "PI-010", "Supplier A d.o.o.",   now.AddDays(-70),  68000.00m,  "Open"),            // overdue
        };
    }

    // Posted purchase invoices — already booked; mostly Paid with a couple still Open.
    private static List<Dictionary<string, object>> GetMockPostedPurchaseInvoiceData()
    {
        var now = DateTime.UtcNow;
        return new List<Dictionary<string, object>>
        {
            Purchase("ppi001", "PPI-001", "Supplier A d.o.o.",   now.AddMonths(-3), 56000.00m,  "Paid"),
            Purchase("ppi002", "PPI-002", "Supplier B d.o.o.",   now.AddMonths(-2), 48000.00m,  "Paid"),
            Purchase("ppi003", "PPI-003", "Materijal Promet",    now.AddMonths(-2), 99000.00m,  "Open"),
            Purchase("ppi004", "PPI-004", "Energo Snabdevanje",  now.AddMonths(-1), 210000.00m, "Paid"),
            Purchase("ppi005", "PPI-005", "Tehno Oprema d.o.o.", now.AddDays(-18),  37500.00m,  "Open"),
            Purchase("ppi006", "PPI-006", "Supplier A d.o.o.",   now.AddDays(-6),   62000.00m,  "Paid"),
        };
    }

    // Open (draft / unposted) purchase credit memos. Credit memos use BC-style status
    // "Open" / "Posted"; the credit-memo normalizer maps these to OPEN / POSTED.
    private static List<Dictionary<string, object>> GetMockPurchaseCreditMemoData()
    {
        var now = DateTime.UtcNow;
        return new List<Dictionary<string, object>>
        {
            Purchase("pcm001", "PCM-001", "Supplier A d.o.o.",  now.AddMonths(-2), 9500.00m, "Open"),
            Purchase("pcm002", "PCM-002", "Materijal Promet",   now.AddMonths(-1), 6200.00m, "Open"),
            Purchase("pcm003", "PCM-003", "Energo Snabdevanje", now.AddDays(-20),  13400.00m, "Posted"),
            Purchase("pcm004", "PCM-004", "Supplier B d.o.o.",  now.AddDays(-8),   4800.00m, "Posted"),
        };
    }

    private static BcCollectionResponse<T> CreateMockVendors<T>(BcQueryOptions? options)
    {
        var vendors = new List<Dictionary<string, object>>
        {
            new() { ["id"] = "v001", ["number"] = "V001", ["displayName"] = "Supplier A d.o.o.",  ["city"] = "Beograd",  ["balance"] = 55000.00m },
            new() { ["id"] = "v002", ["number"] = "V002", ["displayName"] = "Supplier B d.o.o.",  ["city"] = "Novi Sad", ["balance"] = 32000.00m },
            new() { ["id"] = "v003", ["number"] = "V003", ["displayName"] = "Materijal Promet",   ["city"] = "Niš",      ["balance"] = 78000.00m },
            new() { ["id"] = "v004", ["number"] = "V004", ["displayName"] = "Energo Snabdevanje",  ["city"] = "Beograd",  ["balance"] = 144000.00m },
            new() { ["id"] = "v005", ["number"] = "V005", ["displayName"] = "Tehno Oprema d.o.o.", ["city"] = "Kragujevac", ["balance"] = 26000.00m },
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
