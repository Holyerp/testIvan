using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Mapping;
using Xunit;

namespace Pinoles.Api.Tests.Application.Mapping;

public class ItemMapperTests
{
    private readonly ItemMapper _mapper = new();

    [Fact]
    public void Map_CopiesAllFields()
    {
        var source = new BcItem
        {
            Id = "itm001",
            Number = "ITM-001",
            Description = "Cement 25kg",
            Category = "GRAĐEVINA",
            Location = "MAGACIN-1",
            UnitOfMeasure = "KG",
            QuantityOnHand = 120m,
            MinimumStock = 50m,
            UnitCost = 650.00m,
            UnitPrice = 780.00m,
        };

        var dto = _mapper.Map(source);

        Assert.Equal("itm001", dto.Id);
        Assert.Equal("ITM-001", dto.Number);
        Assert.Equal("Cement 25kg", dto.Description);
        Assert.Equal("GRAĐEVINA", dto.Category);
        Assert.Equal("KG", dto.UnitOfMeasure);
        Assert.Equal(120m, dto.QuantityOnHand);
        Assert.Equal(50m, dto.MinimumStock);
        Assert.Equal(650.00m, dto.UnitCost);
    }

    [Fact]
    public void Map_QuantityBelowMinimum_IsLowStockTrue()
    {
        var source = new BcItem
        {
            Id = "itm002",
            Number = "ITM-002",
            Description = "Čelična šipka 12mm",
            QuantityOnHand = 30m,
            MinimumStock = 100m,
        };

        var dto = _mapper.Map(source);

        Assert.True(dto.IsLowStock);
    }

    [Fact]
    public void Map_QuantityAtOrAboveMinimum_IsLowStockFalse()
    {
        // At the threshold (qty == min) is NOT low stock (strictly less-than).
        var atThreshold = _mapper.Map(new BcItem { QuantityOnHand = 50m, MinimumStock = 50m });
        Assert.False(atThreshold.IsLowStock);

        // Above the threshold is not low stock either.
        var above = _mapper.Map(new BcItem { QuantityOnHand = 200m, MinimumStock = 50m });
        Assert.False(above.IsLowStock);
    }

    [Fact]
    public void Map_ProducesNewInstanceEachCall()
    {
        var source = new BcItem { Id = "itm003", Number = "ITM-003", Description = "Crep keramički" };

        var first = _mapper.Map(source);
        var second = _mapper.Map(source);

        Assert.NotSame(first, second);
        Assert.Equal(first.Id, second.Id);
    }
}
