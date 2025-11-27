# 🖥️ CoreDisplay Fleet Platform

> **Advanced Digital Signage Fleet Management System**
> *Scalable. Real-time. Offline-first.*

![Project Status](https://img.shields.io/badge/Status-MVP-blue) ![License](https://img.shields.io/badge/License-MIT-green) ![Build](https://img.shields.io/badge/Build-Passing-brightgreen)

**CoreDisplay** is a comprehensive SaaS platform designed to manage fleets of digital signage devices (Windows Kiosks). It provides real-time monitoring, remote command execution, and content scheduling capabilities, built on a robust .NET 8 microservices architecture.

---

## 🚀 Tech Stack

| Component | Technology | Description |
| :--- | :--- | :--- |
| **Backend** | ![.NET](https://img.shields.io/badge/.NET%208-512BD4?style=flat&logo=dotnet&logoColor=white) | ASP.NET Core Web API, Clean Architecture, CQRS |
| **Database** | ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=flat&logo=postgresql&logoColor=white) | Relational Data & Metadata (EF Core) |
| **Cache** | ![Redis](https://img.shields.io/badge/Redis-DC382D?style=flat&logo=redis&logoColor=white) | Hot Data, Session Management |
| **Messaging** | ![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=flat&logo=rabbitmq&logoColor=white) | Telemetry Ingestion, Command Queues |
| **Frontend** | ![React](https://img.shields.io/badge/React-20232A?style=flat&logo=react&logoColor=61DAFB) | Admin Panel (Vite + TypeScript + TailwindCSS) |
| **Client** | ![Windows](https://img.shields.io/badge/Windows-0078D6?style=flat&logo=windows&logoColor=white) | WPF Kiosk App + CefSharp (Chromium) |
| **Infra** | ![Azure](https://img.shields.io/badge/Azure-0078D4?style=flat&logo=microsoftazure&logoColor=white) | Terraform IaC, Container Apps, Managed DB |

---

## 📚 Documentation

We provide detailed documentation for every role in the team:

### 🛠️ For Developers
*   [**Technical Architecture**](docs/Technical_Architecture.md): Deep dive into the Database Schema, API Endpoints, and Real-time strategy.
*   [**API Overview**](docs/API_Overview.md): Quick reference for REST endpoints.

### 👥 For Users
*   [**User Guide (Admin Panel)**](docs/User_Guide_Admin_Panel.md): Step-by-step manual for managing devices and content.

### ☁️ For DevOps
*   [**Deployment Guide**](docs/Deployment_Guide.md): Instructions for Docker Compose (Local) and Azure Terraform (Cloud) deployment.

---

## ⚡ Quick Start (Local)

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/your-org/coredisplay.git
    cd coredisplay
    ```

2.  **Start Infrastructure**:
    ```bash
    docker-compose up -d --build
    ```

3.  **Access the Platform**:
    *   **Admin Panel**: [http://localhost:3000](http://localhost:3000)
    *   **API Swagger**: [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## 🏗️ Project Structure

```
/
├── iac/                  # Infrastructure as Code (Terraform)
├── src/
│   ├── backend/          # .NET 8 Web API (Clean Architecture)
│   ├── frontend/         # React Admin Panel
│   ├── device/           # Windows WPF Client & Agent
│   └── simulation/       # Device Simulator
├── docs/                 # Documentation
└── docker-compose.yml    # Local Development Stack
```

---

*Built with ❤️ by the CoreDisplay Team.*
