namespace Pinoles.Api.Domain.Constants;

public static class UserRoles
{
    public const string Admin = "ADMIN";
    public const string Manager = "MANAGER";
    public const string Accounting = "ACCOUNTING";
    public const string Warehouse = "WAREHOUSE";

    public static readonly string[] All = { Admin, Manager, Accounting, Warehouse };

    public static bool IsValid(string role) => All.Contains(role);
}
