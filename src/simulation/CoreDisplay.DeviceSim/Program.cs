using System.Net.Http.Json;
using CoreDisplay.Shared.DTOs;

var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5000"); // Adjust port as needed

var hardwareId = Guid.NewGuid().ToString();
Console.WriteLine($"Starting Device Simulator. HardwareId: {hardwareId}");

// Register
var registerDto = new DeviceRegisterDto
{
    HardwareId = hardwareId,
    Name = $"Simulated Device {hardwareId.Substring(0, 4)}",
    OS = "Windows",
    AppVersion = "1.0.0"
};

try
{
    var response = await client.PostAsJsonAsync("/api/v1/devices/register", registerDto);
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Device registered successfully.");
    }
    else
    {
        Console.WriteLine($"Registration failed: {response.StatusCode}");
        return;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting to API: {ex.Message}");
    return;
}

// Heartbeat Loop
var random = new Random();
while (true)
{
    var heartbeat = new HeartbeatDto
    {
        HardwareId = hardwareId,
        Timestamp = DateTime.UtcNow,
        CpuUsage = random.Next(10, 90),
        RamUsage = random.Next(20, 80),
        Temperature = random.Next(30, 70),
        StorageFree = 1024 * 1024 * 1024 * 10L // 10 GB
    };

    try
    {
        var response = await client.PostAsJsonAsync("/api/v1/devices/heartbeat", heartbeat);
        Console.WriteLine($"Heartbeat sent: {response.StatusCode}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error sending heartbeat: {ex.Message}");
    }

    await Task.Delay(10000);
}
