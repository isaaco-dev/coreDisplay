namespace CoreDisplay.Shared.DTOs;

public class CommandDto
{
    public string Type { get; set; } = string.Empty; // "Screenshot", "Reboot", "UpdateConfig"
    public string? Payload { get; set; }
}
