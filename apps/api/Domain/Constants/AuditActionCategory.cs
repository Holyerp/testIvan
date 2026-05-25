namespace Pinoles.Api.Domain.Constants;

/// <summary>
/// Audit-log action CATEGORIES used by the audit-log view filter (US-023). The granular
/// <see cref="AuditActions"/> codes stored in <c>AuditLog.Action</c> are grouped into these
/// four buckets so the admin can filter by activity kind rather than by every code.
///
/// Wire format is SCREAMING_SNAKE_CASE per the cross-layer enum convention.
/// Allowed values: <c>LOGIN | VIEW | EXPORT | ADMIN</c>.
/// </summary>
public static class AuditActionCategory
{
    public const string Login = "LOGIN";
    public const string View = "VIEW";
    public const string Export = "EXPORT";
    public const string Admin = "ADMIN";

    public static readonly string[] All = { Login, View, Export, Admin };

    public static bool IsValid(string category) => All.Contains(category);

    /// <summary>
    /// Map a granular action code to its category. The mapping is prefix/substring-based so
    /// new granular codes fall into the right bucket without a code change:
    /// <list type="bullet">
    /// <item><c>AUTH_LOGIN*</c> -&gt; LOGIN</item>
    /// <item><c>*EXPORT*</c> -&gt; EXPORT</item>
    /// <item><c>*VIEW*</c> -&gt; VIEW</item>
    /// <item><c>ADMIN_*</c> and everything else -&gt; ADMIN (admin/security activity is the default bucket)</item>
    /// </list>
    /// </summary>
    public static string Categorize(string? action)
    {
        if (string.IsNullOrEmpty(action))
            return Admin;

        if (action.StartsWith("AUTH_LOGIN", StringComparison.Ordinal))
            return Login;
        if (action.Contains("EXPORT", StringComparison.Ordinal))
            return Export;
        if (action.Contains("VIEW", StringComparison.Ordinal))
            return View;

        // ADMIN_* and any other security/admin event default to the ADMIN bucket.
        return Admin;
    }

    /// <summary>
    /// The action-code prefix/substring predicate a category filters on, expressed as an EF
    /// Core-translatable check. Returns the set of conditions a query applies for the given
    /// category; null when the category is unknown (caller treats as "no category filter").
    /// </summary>
    public static Func<string, bool>? MatchPredicate(string? category) => category switch
    {
        Login => a => a.StartsWith("AUTH_LOGIN"),
        Export => a => a.Contains("EXPORT"),
        View => a => a.Contains("VIEW"),
        Admin => a => a.StartsWith("ADMIN_"),
        _ => null,
    };
}
