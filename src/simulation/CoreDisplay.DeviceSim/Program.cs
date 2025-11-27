using System.Net.Http.Json;
using System.Text.Json;
using CoreDisplay.Shared.DTOs;

var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5000"); 

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

string? token = null;
Guid? deviceId = null;

try
{
    var response = await client.PostAsJsonAsync("/api/v1/devices/register", registerDto);
    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (content.TryGetProperty("token", out var tokenProp))
        {
            token = tokenProp.GetString();
            Console.WriteLine("Device registered successfully. Token received.");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        if (content.TryGetProperty("deviceId", out var idProp))
        {
            deviceId = Guid.Parse(idProp.GetString()!);
        }
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

// Heartbeat & Command Loop
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
        
        // Poll for commands
        if (deviceId.HasValue)
        {
            try 
            {
                var commandResponse = await client.GetAsync($"/api/v1/devices/{deviceId}/commands");
                if (commandResponse.IsSuccessStatusCode && commandResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    var command = await commandResponse.Content.ReadFromJsonAsync<CommandDto>();
                    if (command != null)
                    {
                        Console.WriteLine($"[COMMAND RECEIVED] Type: {command.Type}, Payload: {command.Payload ?? "None"}");
                        
                        // Simulate Execution
                        switch (command.Type)
                        {
                            case "Screenshot":
                                Console.WriteLine("  -> Capturing Screenshot...");
                                await Task.Delay(500);
                                Console.WriteLine("  -> Uploading to Storage...");
                                break;
                            case "Reboot":
                                Console.WriteLine("  -> Rebooting System...");
                                await Task.Delay(1000);
                                Console.WriteLine("  -> System Restarted.");
                                break;
                            case "Config":
                                Console.WriteLine($"  -> Updating Configuration: {command.Payload}");
                                break;
                        }
                    }
                }
            }
            catch (Exception cmdEx)
            {
                Console.WriteLine($"Error polling commands: {cmdEx.Message}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error sending heartbeat: {ex.Message}");
    }

    await Task.Delay(10000);
}
