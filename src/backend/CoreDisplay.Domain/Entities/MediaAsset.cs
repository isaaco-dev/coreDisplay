using System;

namespace CoreDisplay.Domain.Entities;

public class MediaAsset
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public int? DurationSeconds { get; set; }
}
