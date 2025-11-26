# Guida al Deployment - CoreDisplay Fleet Platform

Questa guida descrive come avviare l'intera piattaforma CoreDisplay (Backend, Frontend, Database) utilizzando Docker Compose, e come configurare l'ambiente di sviluppo per i client Windows.

## Prerequisiti

*   **Docker Desktop** (o Docker Engine + Compose)
*   **.NET 8 SDK**
*   **Node.js 20+** (Opzionale, solo per sviluppo frontend locale senza Docker)
*   **Visual Studio 2022** (Raccomandato per sviluppo Windows/WPF)

## Avvio Rapido (Docker)

Per avviare l'intero stack (Backend, Frontend, Database, Redis, RabbitMQ, MinIO):

1.  Aprire un terminale nella root del progetto.
2.  Eseguire il comando:
    ```bash
    docker-compose up -d --build
    ```
3.  Attendere che tutti i container siano attivi (`docker-compose ps`).

### Accesso ai Servizi

*   **Admin Panel (Frontend)**: [http://localhost:3000](http://localhost:3000)
*   **Backend API**: [http://localhost:5000](http://localhost:5000)
*   **Swagger UI**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
*   **MinIO Console**: [http://localhost:9001](http://localhost:9001) (User: `core_user`, Pass: `core_password`)
*   **RabbitMQ Management**: [http://localhost:15672](http://localhost:15672) (User: `core_user`, Pass: `core_password`)

## Sviluppo Locale (Windows)

Per sviluppare e debuggare i componenti Windows (Kiosk e Agent) o il Backend direttamente su Windows:

1.  Aprire `CoreDisplay.sln` con Visual Studio 2022.
2.  Assicurarsi che i servizi infrastrutturali (Postgres, Redis, ecc.) siano attivi via Docker:
    ```bash
    docker-compose up -d postgres redis rabbitmq minio
    ```
    *(Nota: Non avviare `api` e `admin-panel` da Docker se si intende eseguirli da VS)*.
3.  Impostare `CoreDisplay.Api` come progetto di avvio (o avvio multiplo insieme a `CoreDisplay.DeviceSim` o `CoreDisplay.Windows`).
4.  Premere F5 per avviare il debug.

### Client Windows (Kiosk)

Il progetto `CoreDisplay.Windows` è un'applicazione WPF che utilizza CefSharp.
*   Assicurarsi di avere installato il workload "Sviluppo desktop .NET" in Visual Studio.
*   Il client tenterà di connettersi a `http://localhost:5000` per registrarsi e inviare heartbeat.

### Simulatore

Il progetto `CoreDisplay.DeviceSim` è una console app utile per testare il carico o simulare dispositivi senza hardware Windows.
*   Eseguire più istanze per simulare una flotta.
