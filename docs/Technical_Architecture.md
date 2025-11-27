# Architettura Tecnica - CoreDisplay Fleet Platform

## 1. Schema Database (PostgreSQL)

| Entità | Chiave | Indici Critici | Note |
| :--- | :--- | :--- | :--- |
| **Device** | `Id` (UUID) | `IX_HardwareId`, `IX_TenantId` | Core entity. Contiene Config JSONB. |
| **TelemetryLog** | `Id` (Long) | `IX_DeviceId_Timestamp` | Time-series data (CPU, RAM). |
| **MediaAsset** | `Id` (UUID) | `IX_Hash` | Deduplicazione tramite SHA256. |

## 2. Flusso Dati

### Telemetria
`Device` -> `API (Heartbeat)` -> `Validation` -> `DB (TelemetryLogs)` -> `Redis (LastSeen Update)`

### Comandi
`Admin` -> `API (Command)` -> `Queue (Memory/RabbitMQ)` -> `Device (Polling/MQTT)` -> `Execution`

## 3. Sicurezza & Governance

*   **Auth**: JWT (Admin & Device).
*   **Network**: Isolation via NSG (Azure) / Security Groups (AWS) / VPC Firewall (GCP).
*   **Secrets**: Mai nel codice. Iniettati via Env Vars.

## 4. Multi-Cloud Strategy

L'infrastruttura è definita in Terraform per garantire portabilità.

*   **Azure**: Container Apps + Flexible Postgres.
*   **AWS**: ECS Fargate + RDS.
*   **GCP**: Cloud Run + Cloud SQL.
