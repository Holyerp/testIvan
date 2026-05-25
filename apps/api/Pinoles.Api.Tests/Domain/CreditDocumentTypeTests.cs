using Pinoles.Api.Domain.Constants;
using Xunit;

namespace Pinoles.Api.Tests.Domain;

public class CreditDocumentTypeTests
{
    [Theory]
    [InlineData("CREDIT_MEMO", true)]
    [InlineData("DEBIT_MEMO", true)]
    [InlineData("STORNO", true)]
    [InlineData("INVOICE", false)]
    [InlineData("credit_memo", false)] // case-sensitive wire value
    [InlineData("", false)]
    public void IsValid_ReturnsExpectedResult(string type, bool expected)
    {
        Assert.Equal(expected, CreditDocumentType.IsValid(type));
    }

    [Fact]
    public void All_ContainsThreeTypes()
    {
        Assert.Equal(3, CreditDocumentType.All.Length);
    }

    [Fact]
    public void Constants_HaveCorrectWireValues()
    {
        Assert.Equal("CREDIT_MEMO", CreditDocumentType.CreditMemo);
        Assert.Equal("DEBIT_MEMO", CreditDocumentType.DebitMemo);
        Assert.Equal("STORNO", CreditDocumentType.Storno);
    }
}
