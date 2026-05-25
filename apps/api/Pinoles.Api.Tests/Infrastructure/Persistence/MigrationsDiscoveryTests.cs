using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Pinoles.Api.Infrastructure.Persistence;
using Xunit;

namespace Pinoles.Api.Tests.Infrastructure.Persistence;

/// <summary>
/// Guards against the "MigrateAsync silently skips a migration" bug: a hand-authored
/// migration whose Designer file is missing the [Migration] attribute is invisible to the
/// EF migrator. The app calls MigrateAsync() on startup, so a skipped migration means the
/// new columns never get created in Postgres. These tests assert both migrations are
/// discoverable and ordered correctly. (InMemory tests do not exercise migrations.)
/// </summary>
public class MigrationsDiscoveryTests
{
    private static List<string> DiscoverMigrationIds()
    {
        var asm = typeof(PinolesDbContext).Assembly;
        return asm.GetTypes()
            .Select(t => t.GetCustomAttribute<MigrationAttribute>())
            .Where(a => a != null)
            .Select(a => a!.Id)
            .OrderBy(id => id, StringComparer.Ordinal)
            .ToList();
    }

    [Fact]
    public void BothMigrations_AreDiscoverable_InOrder()
    {
        var ids = DiscoverMigrationIds();

        Assert.Equal(
            new[] { "20260525000000_InitialCreate", "20260525010000_AddUserProfileFields" },
            ids);
    }

    [Fact]
    public void AddUserProfileFields_HasDbContextAttribute()
    {
        var type = typeof(PinolesDbContext).Assembly
            .GetType("Pinoles.Api.Infrastructure.Persistence.Migrations.AddUserProfileFields");

        Assert.NotNull(type);
        Assert.NotNull(type!.GetCustomAttribute<DbContextAttribute>());
        Assert.NotNull(type.GetCustomAttribute<MigrationAttribute>());
    }
}
