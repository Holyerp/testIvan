namespace Pinoles.Api.Application.DTOs;

/// <summary>
/// One audit-log row as rendered on the admin Audit Log screen (US-023). <see cref="Action"/>
/// is the granular SCREAMING_SNAKE code stored in the DB; <see cref="Category"/> is the bucket
/// it maps to (LOGIN | VIEW | EXPORT | ADMIN) for filtering and display. No passwords/tokens
/// are ever surfaced — the audit table only stores the username and a free-text detail string.
/// </summary>
public class AuditLogEntryDto
{
    public string Id { get; set; } = "";
    public string Timestamp { get; set; } = "";   // ISO 8601 CreatedAt (UTC)
    public string? Username { get; set; }
    public string Action { get; set; } = "";       // granular code, e.g. ADMIN_USER_CREATED
    public string Category { get; set; } = "";      // LOGIN | VIEW | EXPORT | ADMIN
    public string? EntityType { get; set; }         // parsed from Details when present, else null
    public string? EntityId { get; set; }           // parsed from Details when present, else null
    public string? Details { get; set; }
}
