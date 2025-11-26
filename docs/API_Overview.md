# Panoramica API - CoreDisplay Fleet Platform

Il Backend espone una REST API documentata via Swagger/OpenAPI.

## Endpoint Chiave

### Gestione Dispositivi (`/api/v1/devices`)

*   **POST** `/register`: Registrazione iniziale del dispositivo (Handshake).
    *   **Body**: `DeviceRegisterDto` (HardwareId, Name, OS, AppVersion).
    *   **Response**: `{ "deviceId": "GUID" }`
*   **POST** `/heartbeat`: Invio telemetria periodica.
    *   **Body**: `HeartbeatDto` (HardwareId, Timestamp, CpuUsage, RamUsage, Temperature, StorageFree).
    *   **Response**: `200 OK`

### Gestione Contenuti (Pianificato)

*   **POST** `/media/upload`: Upload multipart di file media.
*   **GET** `/playlists`: CRUD Playlist.

## Schema Dati

### Telemetry Heartbeat (JSON)

```json
{
  "hardwareId": "string (MAC o UUID)",
  "timestamp": "2023-10-27T10:00:00Z",
  "cpuUsage": 45.5, // Percentuale 0-100
  "ramUsage": 60.2, // Percentuale 0-100
  "temperature": 55.0, // Gradi Celsius
  "storageFree": 1024000000 // Byte
}
```

### Device Configuration (JSONB su DB)

La configurazione del dispositivo è salvata come JSON nel database per flessibilità.

```json
{
  "resolution": "1920x1080",
  "orientation": "landscape", // o "portrait"
  "volume": 80,
  "remoteLogEnabled": true
}
```
