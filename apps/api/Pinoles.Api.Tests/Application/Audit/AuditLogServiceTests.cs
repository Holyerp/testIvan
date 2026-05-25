using Microsoft.EntityFrameworkCore;
using Pinoles.Api.Application.Audit;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Domain.Entities;
using Pinoles.Api.Infrastructure.Persistence;
using Xunit;

namespace Pinoles.Api.Tests.Application.Audit;

public class AuditLogServiceTests
{
    private static PinolesDbContext CreateDbContext()
    {
        var opts = new DbContextOptionsBuilder<PinolesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new PinolesDbContext(opts);
    }

    private static async Task SeedLog(
        PinolesDbContext db,
        string action,
        string username,
        DateTime createdAt,
        string? details = null)
    {
        db.AuditLogs.Add(new AuditLog
        {
            Id = Guid.NewGuid(),
            Action = action,
            UserId = Guid.NewGuid(),
            Username = username,
            Details = details,
            CreatedAt = createdAt,
        });
        await db.SaveChangesAsync();
    }

    /// <summary>Seed a representative spread of rows across categories, users, and dates.</summary>
    private static async Task SeedSpread(PinolesDbContext db)
    {
        var baseDate = new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc);
        await SeedLog(db, AuditActions.AuthLoginSuccess, "alice", baseDate.AddDays(0));
        await SeedLog(db, AuditActions.AdminUserCreated, "bob", baseDate.AddDays(1), "Created user newuser (id=abc-123, role=MANAGER)");
        await SeedLog(db, AuditActions.AdminUserUpdated, "bob", baseDate.AddDays(2), "Updated user newuser (id=abc-123)");
        await SeedLog(db, AuditActions.AdminPasswordReset, "alice", baseDate.AddDays(3), "Reset password for user x (id=def-456)");
        await SeedLog(db, AuditActions.AuthLoginSuccess, "carol", baseDate.AddDays(4));
    }

    [Fact]
    public async Task GetAuditLogAsync_OrdersNewestFirst()
    {
        await using var db = CreateDbContext();
        await SeedSpread(db);
        var service = new AuditLogService(db);

        var result = await service.GetAuditLogAsync(1, 20, null, null, null, null);

        Assert.Equal(5, result.Total);
        Assert.Equal(5, result.Items.Count);
        // Newest first: the carol login (day +4) leads.
        Assert.Equal("carol", result.Items[0].Username);
        Assert.Equal(AuditActions.AuthLoginSuccess, result.Items[0].Action);
    }

    [Fact]
    public async Task GetAuditLogAsync_CategoryAdmin_NarrowsToAdminActions()
    {
        await using var db = CreateDbContext();
        await SeedSpread(db);
        var service = new AuditLogService(db);

        var result = await service.GetAuditLogAsync(1, 20, AuditActionCategory.Admin, null, null, null);

        Assert.Equal(3, result.Total); // created + updated + password reset
        Assert.All(result.Items, e => Assert.Equal(AuditActionCategory.Admin, e.Category));
        Assert.All(result.Items, e => Assert.StartsWith("ADMIN_", e.Action));
    }

    [Fact]
    public async Task GetAuditLogAsync_CategoryLogin_NarrowsToLoginActions()
    {
        await using var db = CreateDbContext();
        await SeedSpread(db);
        var service = new AuditLogService(db);

        var result = await service.GetAuditLogAsync(1, 20, AuditActionCategory.Login, null, null, null);

        Assert.Equal(2, result.Total);
        Assert.All(result.Items, e => Assert.Equal(AuditActionCategory.Login, e.Category));
    }

    [Fact]
    public async Task GetAuditLogAsync_CategoryExport_ReturnsEmpty_WhenNoExportEvents()
    {
        await using var db = CreateDbContext();
        await SeedSpread(db);
        var service = new AuditLogService(db);

        var result = await service.GetAuditLogAsync(1, 20, AuditActionCategory.Export, null, null, null);

        Assert.Equal(0, result.Total);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetAuditLogAsync_UsernameFilter_NarrowsCaseInsensitively()
    {
        await using var db = CreateDbContext();
        await SeedSpread(db);
        var service = new AuditLogService(db);

        var result = await service.GetAuditLogAsync(1, 20, null, "ALICE", null, null);

        Assert.Equal(2, result.Total); // alice login + alice password reset
        Assert.All(result.Items, e => Assert.Equal("alice", e.Username));
    }

    [Fact]
    public async Task GetAuditLogAsync_DateRange_Narrows()
    {
        await using var db = CreateDbContext();
        await SeedSpread(db);
        var service = new AuditLogService(db);

        // Rows are on days 1..5 of May 2026; bound to days 2..3.
        var result = await service.GetAuditLogAsync(
            1, 20, null, null, "2026-05-02", "2026-05-03");

        Assert.Equal(2, result.Total); // created (day +1=May 2) + updated (day +2=May 3)
        Assert.All(result.Items, e =>
        {
            var ts = DateTime.Parse(e.Timestamp, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AdjustToUniversal);
            Assert.True(ts >= new DateTime(2026, 5, 2, 0, 0, 0, DateTimeKind.Utc));
            Assert.True(ts <= new DateTime(2026, 5, 3, 23, 59, 59, DateTimeKind.Utc));
        });
    }

    [Fact]
    public async Task GetAuditLogAsync_PageSizeRespected_AndTotalCorrect()
    {
        await using var db = CreateDbContext();
        await SeedSpread(db);
        var service = new AuditLogService(db);

        var page1 = await service.GetAuditLogAsync(1, 2, null, null, null, null);
        var page2 = await service.GetAuditLogAsync(2, 2, null, null, null, null);

        Assert.Equal(5, page1.Total);
        Assert.Equal(2, page1.Items.Count);
        Assert.Equal(2, page1.PageSize);
        Assert.Equal(2, page2.Items.Count);
        // No overlap between page 1 and page 2 ids.
        Assert.Empty(page1.Items.Select(i => i.Id).Intersect(page2.Items.Select(i => i.Id)));
    }

    [Fact]
    public async Task GetAuditLogAsync_PageSizeClampedToMax()
    {
        await using var db = CreateDbContext();
        await SeedSpread(db);
        var service = new AuditLogService(db);

        var result = await service.GetAuditLogAsync(1, 5000, null, null, null, null);

        Assert.Equal(100, result.PageSize); // clamped to MaxPageSize
    }

    [Fact]
    public async Task GetAuditLogAsync_ComputesCategoryAndParsesEntity()
    {
        await using var db = CreateDbContext();
        await SeedLog(
            db,
            AuditActions.AdminUserCreated,
            "bob",
            new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc),
            "Created user newuser (id=abc-123, role=MANAGER)");
        var service = new AuditLogService(db);

        var result = await service.GetAuditLogAsync(1, 20, null, null, null, null);

        var entry = Assert.Single(result.Items);
        Assert.Equal(AuditActionCategory.Admin, entry.Category);
        Assert.Equal("USER", entry.EntityType);
        Assert.Equal("abc-123", entry.EntityId);
    }

    [Fact]
    public async Task GetAuditLogAsync_LoginEntry_HasNullEntity()
    {
        await using var db = CreateDbContext();
        await SeedLog(db, AuditActions.AuthLoginSuccess, "alice",
            new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc));
        var service = new AuditLogService(db);

        var result = await service.GetAuditLogAsync(1, 20, null, null, null, null);

        var entry = Assert.Single(result.Items);
        Assert.Equal(AuditActionCategory.Login, entry.Category);
        Assert.Null(entry.EntityType);
        Assert.Null(entry.EntityId);
    }

    [Fact]
    public async Task GetAuditLogAsync_NegativeOrZeroPaging_Clamped()
    {
        await using var db = CreateDbContext();
        await SeedSpread(db);
        var service = new AuditLogService(db);

        var result = await service.GetAuditLogAsync(0, 0, null, null, null, null);

        Assert.Equal(1, result.Page);
        Assert.Equal(20, result.PageSize); // default
    }
}
