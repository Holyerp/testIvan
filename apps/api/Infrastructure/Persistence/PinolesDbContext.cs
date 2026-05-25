using Microsoft.EntityFrameworkCore;
using Pinoles.Api.Domain.Entities;

namespace Pinoles.Api.Infrastructure.Persistence;

public class PinolesDbContext : DbContext
{
    public PinolesDbContext(DbContextOptions<PinolesDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure User
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Username).IsUnique();
            e.Property(u => u.Role).HasMaxLength(20).IsRequired();
            e.Property(u => u.Name).HasMaxLength(200).IsRequired();
            e.Property(u => u.Email).HasMaxLength(256);
            e.HasMany(u => u.RefreshTokens)
             .WithOne(r => r.User)
             .HasForeignKey(r => r.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure RefreshToken
        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasIndex(r => r.Token).IsUnique();
        });

        // Configure AuditLog
        modelBuilder.Entity<AuditLog>(e =>
        {
            e.HasKey(a => a.Id);
            e.HasIndex(a => a.CreatedAt);
            e.HasIndex(a => a.UserId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
