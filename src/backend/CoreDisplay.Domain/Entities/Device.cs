using System;
using System.Collections.Generic;
using CoreDisplay.Domain.Enums;

namespace CoreDisplay.Domain.Entities;

public class Device
{
    public Guid Id { get; set; }
    public string HardwareId { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceStatus Status { get; set; }
    public string OS { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
    public DateTime LastSeen { get; set; }
    public string Configuration { get; set; } = "{}"; // JSONB

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<TelemetryLog> TelemetryLogs { get; set; } = new List<TelemetryLog>();
}
