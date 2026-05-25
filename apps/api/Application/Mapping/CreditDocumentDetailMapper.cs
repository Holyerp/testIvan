using Pinoles.Api.Application.Common;
using Pinoles.Api.Application.DTOs;

namespace Pinoles.Api.Application.Mapping;

/// <summary>
/// Maps a raw BC correction document (with expanded line items) to the detail DTO the
/// UI consumes — header, line items, and computed totals (US-016). Reuses
/// <see cref="CreditDocumentMapper.NormalizeType"/> + <see cref="InvoiceStatus"/> for
/// the cross-layer wire values, <see cref="SalesInvoiceLineDto"/> for the line shape,
/// and <see cref="InvoiceTotals"/> for the shared money math (DRY).
/// </summary>
public class CreditDocumentDetailMapper : IBcMapper<BcCreditDocument, CreditDocumentDetailDto>
{
    public CreditDocumentDetailDto Map(BcCreditDocument source)
    {
        var lines = source.SalesInvoiceLines.Select(MapLine).ToList();

        var totals = InvoiceTotals.Compute(
            source.SalesInvoiceLines, l => l.LineAmount, l => l.VatPercent);

        return new CreditDocumentDetailDto
        {
            Header = new CreditDocumentHeaderDto
            {
                Id = source.Id,
                Number = source.Number,
                Type = CreditDocumentMapper.NormalizeType(source.Type),
                PartyName = source.PartyName,
                PostingDate = source.PostingDate,
                OriginalInvoiceNumber = source.OriginalInvoiceNumber,
                Status = InvoiceStatus.NormalizeCreditMemo(source.Status),
            },
            Lines = lines,
            Totals = new SalesInvoiceTotalsDto
            {
                Subtotal = totals.Subtotal,
                VatAmount = totals.VatAmount,
                Total = totals.Total,
            },
        };
    }

    private static SalesInvoiceLineDto MapLine(BcSalesInvoiceLine line) => new()
    {
        Description = line.Description,
        Quantity = line.Quantity,
        UnitPrice = line.UnitPrice,
        VatPercent = line.VatPercent,
        LineTotal = line.LineAmount,
    };
}
