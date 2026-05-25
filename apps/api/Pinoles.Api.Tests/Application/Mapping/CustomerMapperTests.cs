using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Mapping;
using Xunit;

namespace Pinoles.Api.Tests.Application.Mapping;

public class CustomerMapperTests
{
    private readonly CustomerMapper _mapper = new();

    [Fact]
    public void Map_CopiesAllFields()
    {
        var source = new BcCustomer
        {
            Id = "c001",
            Number = "C001",
            DisplayName = "Acme d.o.o.",
            City = "Beograd",
            Balance = 150000.00m,
            BalanceDue = 45000.00m,
        };

        var dto = _mapper.Map(source);

        Assert.Equal("c001", dto.Id);
        Assert.Equal("C001", dto.Number);
        Assert.Equal("Acme d.o.o.", dto.DisplayName);
        Assert.Equal("Beograd", dto.City);
        Assert.Equal(150000.00m, dto.Balance);
        Assert.Equal(45000.00m, dto.BalanceDue);
    }

    [Fact]
    public void Map_HandlesZeroAndEmptyValues()
    {
        var source = new BcCustomer
        {
            Id = "c002",
            Number = "C002",
            DisplayName = "Delta Corp",
            City = string.Empty,
            Balance = 0m,
            BalanceDue = 0m,
        };

        var dto = _mapper.Map(source);

        Assert.Equal(string.Empty, dto.City);
        Assert.Equal(0m, dto.Balance);
        Assert.Equal(0m, dto.BalanceDue);
        Assert.Equal("Delta Corp", dto.DisplayName);
    }

    [Fact]
    public void Map_ProducesNewInstanceEachCall()
    {
        var source = new BcCustomer { Id = "c003", Number = "C003", DisplayName = "Sigma Trade" };

        var first = _mapper.Map(source);
        var second = _mapper.Map(source);

        Assert.NotSame(first, second);
        Assert.Equal(first.Id, second.Id);
    }
}
