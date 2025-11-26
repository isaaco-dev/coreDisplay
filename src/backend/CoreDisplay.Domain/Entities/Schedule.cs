using System;

namespace CoreDisplay.Domain.Entities;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid? DeviceId { get; set; }
    public Device? Device { get; set; }
    public Guid? GroupId { get; set; }
    public Guid PlaylistId { get; set; }
    public Playlist? Playlist { get; set; }
    public string CronExpression { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
