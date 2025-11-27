# 🖥️ CoreDisplay Fleet Platform

> **Advanced Digital Signage Fleet Management System**
> *Scalable. Real-time. Secure.*

![Project Status](https://img.shields.io/badge/Status-MVP-blue) ![License](https://img.shields.io/badge/License-MIT-green) ![Build](https://img.shields.io/badge/Build-Passing-brightgreen) ![Security](https://img.shields.io/badge/Security-Hardened-success)

**CoreDisplay** è una piattaforma SaaS completa progettata per gestire flotte di dispositivi di digital signage (Windows Kiosks). Offre monitoraggio in tempo reale, esecuzione di comandi remoti e pianificazione dei contenuti, costruita su una robusta architettura a microservizi .NET 8.

### ✨ Funzionalità Chiave (Iterazione 1)
*   **Dashboard Avanzata**: Layout moderno a card con indicatori di stato a semaforo (🟢/🔴) e metriche in tempo reale.
*   **Comandi Remoti**: Riavvio, Screenshot e Aggiornamento Configurazione inviati istantaneamente ai dispositivi.
*   **Sicurezza Enterprise**: Autenticazione JWT, Validazione Input rigorosa, Rate Limiting e CSP.
*   **Filtri Avanzati**: Ricerca e filtraggio della flotta per Stato e Sistema Operativo.

---

## 🚀 Tech Stack

| Componente | Tecnologia | Descrizione |
| :--- | :--- | :--- |
| **Backend** | ![.NET](https://img.shields.io/badge/.NET%208-512BD4?style=flat&logo=dotnet&logoColor=white) | ASP.NET Core Web API, Clean Architecture, CQRS |
| **Database** | ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=flat&logo=postgresql&logoColor=white) | Dati Relazionali & Metadati (EF Core) |
| **Cache** | ![Redis](https://img.shields.io/badge/Redis-DC382D?style=flat&logo=redis&logoColor=white) | Hot Data, Gestione Sessioni, Rate Limiting |
| **Frontend** | ![React](https://img.shields.io/badge/React-20232A?style=flat&logo=react&logoColor=61DAFB) | Admin Panel (Vite + TypeScript + TailwindCSS) |
| **Client** | ![Windows](https://img.shields.io/badge/Windows-0078D6?style=flat&logo=windows&logoColor=white) | WPF Kiosk App + CefSharp (Chromium) |
| **Infra** | ![Azure](https://img.shields.io/badge/Azure-0078D4?style=flat&logo=microsoftazure&logoColor=white) | Terraform IaC, Container Apps, Managed DB |

---

## 📚 Documentazione

Offriamo documentazione dettagliata per ogni ruolo del team:

### 🛠️ Per Sviluppatori
*   [**Technical Architecture**](docs/Technical_Architecture.md): Dettagli profondi su Schema DB, API Endpoints, Logica Agent e Variabili d'Ambiente.
*   [**API Overview**](docs/API_Overview.md): Riferimento rapido per gli endpoint REST.

### 👥 Per Utenti
*   [**User Guide (Admin Panel)**](docs/User_Guide_Admin_Panel.md): Manuale passo-passo per gestire dispositivi e contenuti.

### ☁️ Per DevOps
*   [**Deployment Guide**](docs/Deployment_Guide.md): Istruzioni per Docker Compose (Locale) e Azure Terraform (Cloud).

---

## ⚡ Quick Start (Locale)

1.  **Clona il repository**:
    ```bash
    git clone https://github.com/your-org/coredisplay.git
    cd coredisplay
    ```

2.  **Avvia l'Infrastruttura**:
    ```bash
    docker-compose up -d --build
    ```

3.  **Accedi alla Piattaforma**:
    *   **Admin Panel**: [http://localhost:3000](http://localhost:3000)
    *   **API Swagger**: [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## 🏗️ Struttura del Progetto

```
/
├── iac/                  # Infrastructure as Code (Terraform)
├── src/
│   ├── backend/          # .NET 8 Web API (Clean Architecture)
│   ├── frontend/         # React Admin Panel
│   ├── device/           # Windows WPF Client & Agent
│   └── simulation/       # Device Simulator
├── docs/                 # Documentazione Completa
└── docker-compose.yml    # Stack di Sviluppo Locale
```

---

*Built with ❤️ by the CoreDisplay Team.*
