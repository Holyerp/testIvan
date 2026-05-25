using System.Linq;

namespace Pinoles.Api.Domain.Constants;

/// <summary>Single source of truth for module → allowed-roles mapping. Mirrors the
/// authorization policies in Program.cs and the frontend MODULE_ACCESS map.</summary>
public static class ModuleAccess
{
    public static readonly string[] Dashboard = { UserRoles.Admin, UserRoles.Manager, UserRoles.Accounting, UserRoles.Warehouse };
    public static readonly string[] Financial = { UserRoles.Admin, UserRoles.Manager, UserRoles.Accounting };
    public static readonly string[] Warehouse = { UserRoles.Admin, UserRoles.Manager, UserRoles.Warehouse };
    public static readonly string[] Admin = { UserRoles.Admin };

    public static bool CanAccess(string[] moduleRoles, string role) => moduleRoles.Contains(role);
}
