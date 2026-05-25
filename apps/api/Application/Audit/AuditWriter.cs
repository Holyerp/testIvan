using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Domain.Entities;
using Pinoles.Api.Infrastructure.Persistence;

namespace Pinoles.Api.Application.Audit;

/// <summary>
/// EF Core-backed <see cref="IAuditWriter"/>. Adds an <see cref="AuditLog"/> row to the
/// tracked change set; the caller saves it as part of its own transaction so the audit
/// entry and the audited mutation commit together.
/// </summary>
public class AuditWriter : IAuditWriter
{
    private readonly PinolesDbContext _db;

    public AuditWriter(PinolesDbContext db) => _db = db;

    public void Write(
        string action,
        Guid? actorUserId,
        string? actorUsername,
        string? details = null,
        string? ipAddress = null)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            Id = Guid.NewGuid(),
            Action = action,
            UserId = actorUserId,
            Username = actorUsername,
            IpAddress = ipAddress,
            Details = details,
            CreatedAt = DateTime.UtcNow,
        });
    }
}
