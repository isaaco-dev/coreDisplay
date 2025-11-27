# Guida al Deployment - CoreDisplay Fleet Platform

Questa guida tecnica è destinata agli ingegneri DevOps e System Administrators. Copre il deployment locale (Docker) e cloud (Azure Terraform).

## 1. Sviluppo Locale (Docker Compose) 🐳

Il modo più rapido per avviare l'intero stack per test e sviluppo.

### Prerequisiti
*   **Docker Desktop** (o Engine + Compose)
*   **.NET 8 SDK** (Opzionale, per modifiche al codice)

### Istruzioni
1.  **Clona il Repository**:
    ```bash
    git clone <repo-url>
    cd coredisplay
    ```
2.  **Avvia lo Stack**:
    ```bash
    docker-compose up -d --build
    ```
3.  **Verifica Servizi**:
    *   **Admin Panel**: [http://localhost:3000](http://localhost:3000)
    *   **API**: [http://localhost:5000](http://localhost:5000)
    *   **Postgres/Redis**: In esecuzione nei container.

---

## 2. Produzione su Azure (Terraform) ☁️

Utilizziamo **Terraform** per un provisioning Infrastructure-as-Code (IaC) completo e sicuro.

### Architettura Cloud
*   **Compute**: Azure Container Apps (Serverless) per Backend, Frontend e RabbitMQ.
*   **Database**: Azure Database for PostgreSQL (Flexible Server) - Isolato.
*   **Cache**: Azure Cache for Redis.
*   **Storage**: Azure Storage Account.
*   **Client Test**: VM Windows 11 dedicata.

### Prerequisiti
*   **Azure CLI**: `az login` (Autenticato).
*   **Terraform**: v1.0+.

### Procedura di Deployment

1.  **Inizializzazione**:
    ```bash
    cd iac/azure/terraform
    terraform init
    ```

2.  **Pianificazione**:
    Verifica le risorse che verranno create.
    ```bash
    terraform plan -out=tfplan
    ```

3.  **Applicazione**:
    Provisioning delle risorse (Tempo stimato: 15-20 min).
    ```bash
    terraform apply tfplan
    ```

### Post-Deployment & Accesso

Al termine, Terraform restituirà gli output critici. Puoi recuperarli in qualsiasi momento con:
```bash
terraform output
```

| Output | Descrizione |
| :--- | :--- |
| `backend_url` | URL pubblico API HTTPS (es. `https://app-backend...`) |
| `frontend_url` | URL pubblico Admin Panel |
| `vm_public_ip` | IP per accesso RDP alla VM di test |
| `vm_username` | Credenziali Admin VM |

### Configurazione VM Client
1.  Connettiti via RDP alla `vm-client` usando le credenziali fornite.
2.  Installa il client `CoreDisplay.Windows`.
3.  Modifica `appsettings.json` puntando al `backend_url`.
4.  Avvia l'applicazione. Dovrebbe apparire "Online" nella Dashboard.
