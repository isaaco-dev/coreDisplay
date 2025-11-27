# 🖥️ CoreDisplay Fleet Platform

> **Advanced Digital Signage Fleet Management System**
> *Scalable. Real-time. Secure. Multi-Cloud.*

![Project Status](https://img.shields.io/badge/Status-MVP-blue) ![License](https://img.shields.io/badge/License-MIT-green) ![Build](https://img.shields.io/badge/Build-Passing-brightgreen) ![Security](https://img.shields.io/badge/Security-Hardened-success)

**CoreDisplay** è una piattaforma SaaS completa per la gestione di flotte di digital signage. Offre monitoraggio real-time, comandi remoti e supporto Multi-Cloud nativo (Azure, AWS, GCP).

### ✨ Funzionalità Chiave
*   **Dashboard Avanzata**: UI moderna con stato real-time (🟢/🔴).
*   **Comandi Remoti**: Riavvio, Screenshot, Configurazione.
*   **Sicurezza**: JWT, Rate Limiting, CSP, Network Isolation.
*   **Multi-Cloud IaC**: Terraform per Azure, AWS e GCP.

---

## 🚀 Tech Stack

| Componente | Tecnologia | Descrizione |
| :--- | :--- | :--- |
| **Backend** | ![.NET](https://img.shields.io/badge/.NET%208-512BD4?style=flat&logo=dotnet&logoColor=white) | ASP.NET Core Web API, Clean Architecture |
| **Database** | ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=flat&logo=postgresql&logoColor=white) | Relational Data (EF Core) |
| **Frontend** | ![React](https://img.shields.io/badge/React-20232A?style=flat&logo=react&logoColor=61DAFB) | Admin Panel (Vite + TS + Tailwind) |
| **Infra** | ![Terraform](https://img.shields.io/badge/Terraform-7B42BC?style=flat&logo=terraform&logoColor=white) | IaC per Azure, AWS, GCP |

---

## 📚 Documentazione

*   [**Technical Architecture**](docs/Technical_Architecture.md): Dettagli tecnici profondi.
*   [**User Guide**](docs/User_Guide_Admin_Panel.md): Manuale utente.
*   [**Deployment Guide**](docs/Deployment_Guide.md): Istruzioni Multi-Cloud.
*   [**Contributing**](CONTRIBUTING.md): Linee guida per contribuire.

---

## ⚡ Quick Start

```bash
# 1. Clona
git clone https://github.com/your-org/coredisplay.git

# 2. Avvia (Locale)
docker-compose up -d --build

# 3. Accedi
# Admin: http://localhost:3000
# API: http://localhost:5000
```

---

*Copyright (c) 2025 CoreDisplay Team. Licensed under MIT.*
