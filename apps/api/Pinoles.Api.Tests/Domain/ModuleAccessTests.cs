using Pinoles.Api.Domain.Constants;
using Xunit;

namespace Pinoles.Api.Tests.Domain;

public class ModuleAccessTests
{
    [Theory]
    // Dashboard — all four roles allowed
    [InlineData(UserRoles.Admin, true)]
    [InlineData(UserRoles.Manager, true)]
    [InlineData(UserRoles.Accounting, true)]
    [InlineData(UserRoles.Warehouse, true)]
    public void Dashboard_AllowsEveryRole(string role, bool expected)
    {
        Assert.Equal(expected, ModuleAccess.CanAccess(ModuleAccess.Dashboard, role));
    }

    [Theory]
    [InlineData(UserRoles.Admin, true)]
    [InlineData(UserRoles.Manager, true)]
    [InlineData(UserRoles.Accounting, true)]
    [InlineData(UserRoles.Warehouse, false)]
    public void Financial_ExcludesWarehouse(string role, bool expected)
    {
        Assert.Equal(expected, ModuleAccess.CanAccess(ModuleAccess.Financial, role));
    }

    [Theory]
    [InlineData(UserRoles.Admin, true)]
    [InlineData(UserRoles.Manager, true)]
    [InlineData(UserRoles.Warehouse, true)]
    [InlineData(UserRoles.Accounting, false)]
    public void Warehouse_ExcludesAccounting(string role, bool expected)
    {
        Assert.Equal(expected, ModuleAccess.CanAccess(ModuleAccess.Warehouse, role));
    }

    [Theory]
    [InlineData(UserRoles.Admin, true)]
    [InlineData(UserRoles.Manager, false)]
    [InlineData(UserRoles.Accounting, false)]
    [InlineData(UserRoles.Warehouse, false)]
    public void Admin_AllowsOnlyAdmin(string role, bool expected)
    {
        Assert.Equal(expected, ModuleAccess.CanAccess(ModuleAccess.Admin, role));
    }

    [Fact]
    public void Warehouse_HasNoFinancialAccess_AndNoAdminAccess()
    {
        Assert.False(ModuleAccess.CanAccess(ModuleAccess.Financial, UserRoles.Warehouse));
        Assert.False(ModuleAccess.CanAccess(ModuleAccess.Admin, UserRoles.Warehouse));
        Assert.True(ModuleAccess.CanAccess(ModuleAccess.Warehouse, UserRoles.Warehouse));
        Assert.True(ModuleAccess.CanAccess(ModuleAccess.Dashboard, UserRoles.Warehouse));
    }

    [Fact]
    public void Accounting_HasFinancialAndDashboard_ButNoWarehouse()
    {
        Assert.True(ModuleAccess.CanAccess(ModuleAccess.Financial, UserRoles.Accounting));
        Assert.True(ModuleAccess.CanAccess(ModuleAccess.Dashboard, UserRoles.Accounting));
        Assert.False(ModuleAccess.CanAccess(ModuleAccess.Warehouse, UserRoles.Accounting));
    }

    [Fact]
    public void AdminAndManager_AccessFinancialWarehouseDashboard()
    {
        foreach (var role in new[] { UserRoles.Admin, UserRoles.Manager })
        {
            Assert.True(ModuleAccess.CanAccess(ModuleAccess.Financial, role));
            Assert.True(ModuleAccess.CanAccess(ModuleAccess.Warehouse, role));
            Assert.True(ModuleAccess.CanAccess(ModuleAccess.Dashboard, role));
        }
    }

    [Fact]
    public void CanAccess_ReturnsFalse_ForUnknownRole()
    {
        Assert.False(ModuleAccess.CanAccess(ModuleAccess.Dashboard, "SUPERUSER"));
        Assert.False(ModuleAccess.CanAccess(ModuleAccess.Financial, ""));
    }
}
