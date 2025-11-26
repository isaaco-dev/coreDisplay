using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CoreDisplay.Shared.DTOs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreDisplay.Windows;

public class HeartbeatService : BackgroundService
{
    private readonly ILogger<HeartbeatService> _logger;
    private readonly HttpClient _client;
    private readonly string _hardwareId;

    public HeartbeatService(ILogger<HeartbeatService> logger)
    {
        _logger = logger;
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
        _hardwareId = Guid.NewGuid().ToString(); // In real app, persist this
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Heartbeat Service starting...");

        // Register first
        try
        {
            var registerDto = new DeviceRegisterDto
            {
                HardwareId = _hardwareId,
                Name = Environment.MachineName,
                OS = "Windows",
                AppVersion = "1.0.0"
            };
            await _client.PostAsJsonAsync("/api/v1/devices/register", registerDto, stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register device");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var heartbeat = new HeartbeatDto
                {
                    HardwareId = _hardwareId,
                    Timestamp = DateTime.UtcNow,
                    CpuUsage = 0, // Implement actual metrics
                    RamUsage = 0,
                    Temperature = 0,
                    StorageFree = 0
                };

                await _client.PostAsJsonAsync("/api/v1/devices/heartbeat", heartbeat, stoppingToken);
                _logger.LogInformation("Heartbeat sent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send heartbeat");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
