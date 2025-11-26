using CoreDisplay.Application.Common.Interfaces;
using CoreDisplay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreDisplay.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<MediaAsset> MediaAssets { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<TelemetryLog> TelemetryLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Device>()
            .HasIndex(d => new { d.TenantId, d.HardwareId })
            .IsUnique();

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.Devices)
            .WithOne(d => d.Tenant)
            .HasForeignKey(d => d.TenantId);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.MediaAssets)
            .WithOne(m => m.Tenant)
            .HasForeignKey(m => m.TenantId);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.Playlists)
            .WithOne(p => p.Tenant)
            .HasForeignKey(p => p.TenantId);
            
        // Configure TelemetryLog as a potential hypertable candidate (standard table for now)
        modelBuilder.Entity<TelemetryLog>()
            .HasIndex(t => t.Timestamp);
    }
}
