using CoreDisplay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CoreDisplay.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<Device> Devices { get; }
    DbSet<MediaAsset> MediaAssets { get; }
    DbSet<Playlist> Playlists { get; }
    DbSet<Schedule> Schedules { get; }
    DbSet<TelemetryLog> TelemetryLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
