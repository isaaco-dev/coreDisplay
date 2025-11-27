# Architettura Tecnica - CoreDisplay Fleet Platform

Questo documento fornisce un'analisi tecnica approfondita della piattaforma CoreDisplay, destinata a Senior Developers e System Architects.

## 1. Schema Database (PostgreSQL)

Il sistema utilizza **Entity Framework Core (Code First)**. Di seguito i dettagli critici delle entità.

### Entità Core

| Entità | Chiave Primaria | Campi Chiave | Relazioni | Indici Critici |
| :--- | :--- | :--- | :--- | :--- |
| **Tenant** | `Id` (UUID) | `ApiKey` (Encrypted) | 1:N Devices | `IX_Tenants_ApiKey` |
| **Device** | `Id` (UUID) | `HardwareId`, `Status`, `Configuration` (JSONB) | N:1 Tenant | `IX_Devices_HardwareId` (Unique), `IX_Devices_TenantId` |
| **TelemetryLog** | `Id` (Long) | `Timestamp`, `CpuUsage`, `RamUsage` | N:1 Device | `IX_TelemetryLogs_DeviceId_Timestamp` (Time-series optimization) |
| **MediaAsset** | `Id` (UUID) | `Hash` (SHA256), `StoragePath` | N:1 Tenant | `IX_MediaAssets_Hash` |

> **Nota**: `TelemetryLog` è progettata per essere migrata su una iper-tabella TimescaleDB nella fase Enterprise per gestire l'alta cardinalità.

---

## 2. Architettura Concettuale & Flusso Dati

La piattaforma segue un'architettura a microservizi (o modulare monolitica per l'MVP) con una chiara separazione delle responsabilità.

### Flusso: Telemetria (Device -> Cloud)
1.  **Device Agent**: Raccoglie metriche (CPU, RAM) ogni 30s.
2.  **API (Heartbeat)**: Riceve il payload JSON autenticato via JWT.
3.  **Backend**: Valida il token, aggiorna lo stato `LastSeen` del Device (Redis/DB) e persiste il log in `TelemetryLogs`.

### Flusso: Comandi (Cloud -> Device)
1.  **Admin Panel**: L'utente clicca "Reboot".
2.  **API**: Riceve la richiesta POST, valida i permessi Admin.
3.  **Command Queue**: Il comando viene accodato (In-Memory per MVP, RabbitMQ per Prod).
4.  **Device Agent**: Esegue il polling (o riceve via MQTT) del comando, lo esegue localmente e (opzionale) invia una conferma.

---

## 3. Logica Device Agent (Ciclo di Vita)

Il client Windows (WPF/Service) segue una macchina a stati rigorosa per garantire resilienza.

```mermaid
graph TD
    A[Start] --> B{Token Valido?}
    B -- No --> C[Registrazione (Handshake)]
    C --> D[Salva Token Sicuro (DPAPI)]
    D --> E[Loop Principale]
    B -- Si --> E
    E --> F[Raccolta Telemetria]
    F --> G[Invio Heartbeat (API)]
    G --> H{Comandi Pendenti?}
    H -- Si --> I[Esegui Comando]
    I --> E
    H -- No --> E
```

1.  **Handshake**: All'avvio, se non ha un token, chiama `/register` con il suo `HardwareId`.
2.  **Secure Storage**: Il token JWT ricevuto viene cifrato con DPAPI e salvato localmente.
3.  **Heartbeat Loop**: Ogni 30s invia lo stato. Se riceve 401 Unauthorized, tenta un refresh (nuova registrazione).

---

## 4. Variabili d'Ambiente (Configurazione)

Tutti i servizi sono configurati tramite Variabili d'Ambiente, iniettate da Azure Container Apps o Docker Compose.

### Backend (`app-backend`)

| Variabile | Descrizione | Esempio / Valore Default |
| :--- | :--- | :--- |
| `ConnectionStrings__DefaultConnection` | Stringa connessione PostgreSQL | `Host=db;Database=core;...` |
| `Redis__ConnectionString` | Endpoint Redis | `redis:6379` |
| `Jwt__Key` | Chiave segreta per firma Token | *(Secret)* |
| `ASPNETCORE_ENVIRONMENT` | Ambiente di esecuzione | `Development` / `Production` |

### Frontend (`app-frontend`)

| Variabile | Descrizione | Esempio |
| :--- | :--- | :--- |
| `VITE_API_URL` | URL pubblico del Backend | `https://api.coredisplay.com` |

---

## 5. Convenzioni di Codice

*   **Backend**: Clean Architecture. I Controller sono sottili, la logica di business risiede in `Application`.
*   **Validazione**: FluentValidation è obbligatorio per ogni DTO di input.
*   **Sicurezza**: Mai committare segreti. Usare `UserSecrets` in locale e `KeyVault`/`EnvVars` in produzione.
