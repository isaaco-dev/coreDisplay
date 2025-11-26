using System;

namespace CoreDisplay.Domain.Entities;

public class TelemetryLog
{
    public long Id { get; set; }
    public Guid DeviceId { get; set; }
    public Device? Device { get; set; }
    public DateTime Timestamp { get; set; }
    public float CpuUsage { get; set; }
    public float RamUsage { get; set; }
    public float Temperature { get; set; }
    public long StorageFree { get; set; }
}
