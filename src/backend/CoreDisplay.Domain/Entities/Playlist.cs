using System;
using System.Collections.Generic;

namespace CoreDisplay.Domain.Entities;

public class Playlist
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Items { get; set; } = "[]"; // JSONB

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
