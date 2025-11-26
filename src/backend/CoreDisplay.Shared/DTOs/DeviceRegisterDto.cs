using System;

namespace CoreDisplay.Shared.DTOs;

public class DeviceRegisterDto
{
    public string HardwareId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string OS { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
}
