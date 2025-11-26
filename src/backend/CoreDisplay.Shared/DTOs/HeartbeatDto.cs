using System;

namespace CoreDisplay.Shared.DTOs;

public class HeartbeatDto
{
    public string HardwareId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public float CpuUsage { get; set; }
    public float RamUsage { get; set; }
    public float Temperature { get; set; }
    public long StorageFree { get; set; }
}
