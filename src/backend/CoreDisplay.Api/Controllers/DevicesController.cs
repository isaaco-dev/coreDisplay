using CoreDisplay.Application.Common.Interfaces;
using CoreDisplay.Domain.Entities;
using CoreDisplay.Domain.Enums;
using CoreDisplay.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreDisplay.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DevicesController> _logger;

    public DevicesController(IApplicationDbContext context, ILogger<DevicesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] DeviceRegisterDto dto)
    {
        _logger.LogInformation("Registering device: {HardwareId}", dto.HardwareId);

        // In a real app, we would authenticate the tenant here.
        // For now, we'll assume a default tenant or create one if none exists.
        var tenant = await _context.Tenants.FirstOrDefaultAsync();
        if (tenant == null)
        {
            tenant = new Tenant { Name = "Default Tenant", ApiKey = Guid.NewGuid().ToString() };
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        var device = await _context.Devices.FirstOrDefaultAsync(d => d.HardwareId == dto.HardwareId);
        if (device == null)
        {
            device = new Device
            {
                HardwareId = dto.HardwareId,
                TenantId = tenant.Id,
                Name = dto.Name,
                Status = DeviceStatus.Online,
                OS = dto.OS,
                AppVersion = dto.AppVersion,
                LastSeen = DateTime.UtcNow
            };
            _context.Devices.Add(device);
        }
        else
        {
            device.Name = dto.Name;
            device.Status = DeviceStatus.Online;
            device.OS = dto.OS;
            device.AppVersion = dto.AppVersion;
            device.LastSeen = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(CancellationToken.None);
        return Ok(new { DeviceId = device.Id });
    }

    [HttpPost("heartbeat")]
    public async Task<IActionResult> Heartbeat([FromBody] HeartbeatDto dto)
    {
        _logger.LogInformation("Heartbeat from: {HardwareId}", dto.HardwareId);

        var device = await _context.Devices.FirstOrDefaultAsync(d => d.HardwareId == dto.HardwareId);
        if (device == null)
        {
            return NotFound("Device not found. Please register first.");
        }

        device.LastSeen = DateTime.UtcNow;
        device.Status = DeviceStatus.Online;

        var log = new TelemetryLog
        {
            DeviceId = device.Id,
            Timestamp = dto.Timestamp,
            CpuUsage = dto.CpuUsage,
            RamUsage = dto.RamUsage,
            Temperature = dto.Temperature,
            StorageFree = dto.StorageFree
        };

        _context.TelemetryLogs.Add(log);
        await _context.SaveChangesAsync(CancellationToken.None);

        return Ok();
    }
}
