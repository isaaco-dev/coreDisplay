using System.Net.Http.Json;
using CoreDisplay.Shared.DTOs;

namespace CoreDisplay.Agent;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _client;
    private readonly string _hardwareId;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
        _hardwareId = Guid.NewGuid().ToString();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Agent Service running at: {time}", DateTimeOffset.Now);

        // Register
        try 
        {
            var registerDto = new DeviceRegisterDto
            {
                HardwareId = _hardwareId,
                Name = Environment.MachineName + " Agent",
                OS = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                AppVersion = "1.0.0"
            };
            await _client.PostAsJsonAsync("/api/v1/devices/register", registerDto, stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Agent sending heartbeat at: {time}", DateTimeOffset.Now);
            }

            try
            {
                var heartbeat = new HeartbeatDto
                {
                    HardwareId = _hardwareId,
                    Timestamp = DateTime.UtcNow,
                    CpuUsage = 0, 
                    RamUsage = 0,
                    Temperature = 0,
                    StorageFree = 0
                };
                await _client.PostAsJsonAsync("/api/v1/devices/heartbeat", heartbeat, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Heartbeat failed");
            }

            await Task.Delay(30000, stoppingToken);
        }
    }
}
