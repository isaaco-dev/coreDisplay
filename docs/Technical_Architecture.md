# Technical Architecture - CoreDisplay Fleet Platform

This document provides a deep technical dive into the CoreDisplay platform, intended for senior developers and architects.

## 1. Database Schema (PostgreSQL)

The system uses **Entity Framework Core (Code First)** to map the following domain entities.

### Core Entities

| Entity | Description | Key Fields | Relationships |
| :--- | :--- | :--- | :--- |
| **Tenant** | Multi-tenancy root | `Id` (UUID), `ApiKey` (Encrypted) | 1:N Devices, 1:N MediaAssets |
| **Device** | Physical Player | `HardwareId` (Unique), `Status` (Enum), `Configuration` (JSONB) | N:1 Tenant, 1:N TelemetryLogs |
| **MediaAsset** | Content Files | `StoragePath`, `Hash` (SHA256), `MimeType` | N:1 Tenant |
| **Playlist** | Ordered Content | `Items` (JSONB - Sequence of Media IDs) | N:1 Tenant, 1:N Schedules |
| **Schedule** | Playback Rules | `CronExpression`, `StartDate`, `EndDate` | N:1 Device OR Group, N:1 Playlist |
| **TelemetryLog** | Time-series Data | `Timestamp`, `CpuUsage`, `RamUsage` | N:1 Device |

> **Note**: `TelemetryLog` is designed to be migrated to a TimescaleDB hypertable in the Enterprise phase for high-cardinality performance.

---

## 2. API Specification (REST)

The Backend exposes a RESTful API documented via OpenAPI (Swagger).

### Base URL
`http://localhost:5000/api/v1`

### Key Endpoints

#### Device Management
*   **POST** `/devices/register`
    *   **Purpose**: Initial handshake and provisioning.
    *   **Request DTO**:
        ```json
        {
          "hardwareId": "string",
          "name": "string",
          "os": "string",
          "appVersion": "string"
        }
        ```
    *   **Response**: `{ "deviceId": "guid" }`

*   **POST** `/devices/heartbeat`
    *   **Purpose**: Periodic status update (every 30s).
    *   **Request DTO**:
        ```json
        {
          "hardwareId": "string",
          "timestamp": "ISO8601",
          "cpuUsage": float,
          "ramUsage": float,
          "temperature": float,
          "storageFree": long
        }
        ```

#### Content Management (Planned)
*   **POST** `/media/upload`: Multipart/form-data upload to Object Storage (MinIO/S3).
*   **GET** `/playlists`: Retrieve playlists for a tenant.

---

## 3. Real-time Strategy

CoreDisplay uses a hybrid approach for real-time communication:

### Telemetry Ingestion (Device -> Cloud)
*   **Protocol**: HTTP (MVP) -> MQTT over TLS (Enterprise).
*   **Flow**: Devices publish telemetry to a message broker (RabbitMQ/Azure Service Bus). A background worker consumes these messages and persists them to the DB (bulk insert).
*   **Topic**: `core/devices/{deviceId}/telemetry`

### Command & Control (Cloud -> Device)
*   **Protocol**: WebSocket / MQTT Sub.
*   **Flow**: The Admin Panel sends a command (e.g., "Reboot") via API. The API publishes a message to the device's specific command topic.
*   **Topic**: `core/devices/{deviceId}/commands`

---

## 4. Code Conventions

### Backend (.NET 8)
*   **Architecture**: Clean Architecture (Domain, Application, Infrastructure, Api).
*   **Pattern**: CQRS (Command Query Responsibility Segregation) using MediatR (planned).
*   **Validation**: FluentValidation for all incoming DTOs.
*   **Logging**: Serilog (Structured Logging).
*   **Naming**: PascalCase for public members, _camelCase for private fields.

### Frontend (React)
*   **State Management**: TanStack Query (React Query) for server state.
*   **Styling**: TailwindCSS.
*   **Types**: TypeScript interfaces must strictly mirror Backend DTOs.
