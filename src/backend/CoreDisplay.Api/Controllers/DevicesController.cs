using CoreDisplay.Application.Common.Interfaces;
using CoreDisplay.Domain.Entities;
using CoreDisplay.Domain.Enums;
using CoreDisplay.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace CoreDisplay.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize] // Require JWT by default
public class DevicesController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DevicesController> _logger;
    private const string SecretKey = "SuperSecretKeyForDevEnvironmentOnly123!"; // Move to config in prod

    public DevicesController(IApplicationDbContext context, ILogger<DevicesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetDevices([FromQuery] DeviceStatus? status, [FromQuery] string? os)
    {
        // PERFORMANCE: Use AsNoTracking for read-only queries to avoid change tracking overhead
        var query = _context.Devices.AsNoTracking().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(d => d.Status == status.Value);
        }

        if (!string.IsNullOrEmpty(os))
        {
            query = query.Where(d => d.OS.Contains(os));
        }

        var devices = await query.ToListAsync();
        return Ok(devices);
    }

    [HttpPost("register")]
    [AllowAnonymous] // Initial handshake must be open
    public async Task<IActionResult> Register([FromBody] DeviceRegisterDto dto)
    {
        _logger.LogInformation("Registering device: {HardwareId}", dto.HardwareId);

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

        // Generate JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", device.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddHours(1), // Short-lived
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { DeviceId = device.Id, Token = tokenString });
    }

    [HttpPost("heartbeat")]
    public async Task<IActionResult> Heartbeat([FromBody] HeartbeatDto dto)
    {
        // In a real scenario, validate the JWT from the Authorization header here.
        // var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
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

    // In-memory queue for MVP command delivery (Replace with RabbitMQ/MQTT in Production)
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, Queue<CommandDto>> CommandQueues = new();

    [HttpPost("{id}/command")]
    public async Task<IActionResult> SendCommand(Guid id, [FromBody] CommandDto command)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null) return NotFound();

        _logger.LogInformation("Queuing command {Command} for device {DeviceId}", command.Type, id);

        var queue = CommandQueues.GetOrAdd(id, _ => new Queue<CommandDto>());
        queue.Enqueue(command);
        
        return Accepted(new { Message = $"Command {command.Type} queued for device {id}" });
    }

    [HttpGet("{id}/commands")]
    public IActionResult GetPendingCommands(Guid id)
    {
        if (CommandQueues.TryGetValue(id, out var queue) && queue.Count > 0)
        {
            var command = queue.Dequeue();
            _logger.LogInformation("Device {DeviceId} retrieved command {Command}", id, command.Type);
            return Ok(command);
        }
        
        return NoContent();
    }
}
