using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Mapping;
using Xunit;

namespace Pinoles.Api.Tests.Application.Mapping;

public class VendorMapperTests
{
    private readonly VendorMapper _mapper = new();

    [Fact]
    public void Map_CopiesAllFields()
    {
        var source = new BcVendor
        {
            Id = "v001",
            Number = "V001",
            DisplayName = "Supplier A d.o.o.",
            City = "Beograd",
            Balance = 55000.00m,
            Phone = "+381 11 2345678",
        };

        var dto = _mapper.Map(source);

        Assert.Equal("v001", dto.Id);
        Assert.Equal("V001", dto.Number);
        Assert.Equal("Supplier A d.o.o.", dto.DisplayName);
        Assert.Equal("Beograd", dto.City);
        Assert.Equal(55000.00m, dto.Balance);
        Assert.Equal("+381 11 2345678", dto.Phone);
    }

    [Fact]
    public void Map_HandlesZeroAndEmptyValues()
    {
        var source = new BcVendor
        {
            Id = "v002",
            Number = "V002",
            DisplayName = "Supplier B d.o.o.",
            City = string.Empty,
            Balance = 0m,
            Phone = string.Empty,
        };

        var dto = _mapper.Map(source);

        Assert.Equal(string.Empty, dto.City);
        Assert.Equal(0m, dto.Balance);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal("Supplier B d.o.o.", dto.DisplayName);
    }

    [Fact]
    public void Map_ProducesNewInstanceEachCall()
    {
        var source = new BcVendor { Id = "v003", Number = "V003", DisplayName = "Materijal Promet" };

        var first = _mapper.Map(source);
        var second = _mapper.Map(source);

        Assert.NotSame(first, second);
        Assert.Equal(first.Id, second.Id);
    }
}
