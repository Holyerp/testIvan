using Pinoles.Api.Application.Interfaces;

namespace Pinoles.Api.Presentation.Endpoints;

/// <summary>
/// Unified credit-documents endpoints (US-016): list + detail across credit memos,
/// debit memos, and storno invoices. Financial module (RequireFinancial); WAREHOUSE
/// is denied (403). See docs/api/credit-documents.md for the contract.
/// </summary>
public static class CreditDocumentsEndpoints
{
    public static void MapCreditDocumentsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/credit-documents")
            .WithTags("CreditDocuments")
            .RequireAuthorization("RequireFinancial");

        group.MapGet("/", GetCreditDocuments);
        group.MapGet("/{id}", GetCreditDocumentById);
    }

    private const string NotFoundCreditDocument = "NOT_FOUND_CREDIT_DOCUMENT";

    private static async Task<IResult> GetCreditDocuments(
        ICreditDocumentService service,
        int? page,
        int? pageSize,
        string? search,
        string? sortBy,
        string? sortDir,
        string? type,
        string? fromDate,
        string? toDate,
        CancellationToken ct)
    {
        try
        {
            var result = await service.GetCreditDocumentsAsync(
                page ?? 1, pageSize ?? 20, search, sortBy, sortDir, type, fromDate, toDate, ct);
            return Results.Ok(new { success = true, data = result });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch credit documents", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }

    private static async Task<IResult> GetCreditDocumentById(
        ICreditDocumentService service, string id, CancellationToken ct)
    {
        try
        {
            var detail = await service.GetCreditDocumentByIdAsync(id, ct);
            if (detail == null)
            {
                return Results.Json(
                    new { success = false, error = "Credit document not found", code = NotFoundCreditDocument },
                    statusCode: 404);
            }

            return Results.Ok(new { success = true, data = detail });
        }
        catch
        {
            return Results.Json(
                new { success = false, error = "Failed to fetch credit document", code = "INTEGRATION_BC_UNAVAILABLE" },
                statusCode: 502);
        }
    }
}
