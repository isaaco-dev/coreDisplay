# Deployment Guide - CoreDisplay Fleet Platform

This guide is intended for DevOps Engineers and System Administrators. It covers both local development setup and production deployment on Azure using Terraform.

## 1. Local Development (Docker Compose)

The fastest way to spin up the environment for testing or development.

### Prerequisites
*   Docker Desktop
*   .NET 8 SDK (optional, for code edits)

### Steps
1.  **Clone the Repo**:
    ```bash
    git clone <repo-url>
    cd coredisplay
    ```
2.  **Start Services**:
    ```bash
    docker-compose up -d --build
    ```
3.  **Verify**:
    *   Admin Panel: `http://localhost:3000`
    *   API: `http://localhost:5000`
    *   DB/Redis/RabbitMQ: Running in background containers.

---

## 2. Azure Production Deployment (Terraform)

We use **Terraform** to provision a complete, production-ready environment on Azure.

### Architecture
*   **Compute**: Azure Container Apps (Serverless Containers) for Backend, Frontend, and RabbitMQ.
*   **Database**: Azure Database for PostgreSQL (Flexible Server).
*   **Cache**: Azure Cache for Redis.
*   **Storage**: Azure Storage Account.
*   **Test Client**: A Windows 11 VM for validating the Kiosk app.

### Prerequisites
*   **Azure CLI**: `az login` (Must be authenticated).
*   **Terraform**: Installed (v1.0+).

### Deployment Steps

1.  **Navigate to IaC Directory**:
    ```bash
    cd iac/azure/terraform
    ```

2.  **Initialize Terraform**:
    Downloads the Azure provider plugins.
    ```bash
    terraform init
    ```

3.  **Review Plan**:
    See what resources will be created.
    ```bash
    terraform plan -out=tfplan
    ```

4.  **Apply Infrastructure**:
    Provision the resources (this may take 15-20 mins).
    ```bash
    terraform apply tfplan
    ```

### Post-Deployment

After a successful apply, Terraform will output critical connection details. You can retrieve them anytime with:
```bash
terraform output
```

**Key Outputs:**
*   `backend_url`: The public HTTPS URL of your API (e.g., `https://app-backend.polartree-xyz.eastus.azurecontainerapps.io`).
*   `frontend_url`: The public URL of the Admin Panel.
*   `vm_public_ip`: IP address to RDP into the Windows Test VM.
*   `vm_username` / `vm_password`: Credentials for the VM.

### Connecting the Windows Client
1.  RDP into the `vm-client` using the output credentials.
2.  Deploy the `CoreDisplay.Windows` application to the VM.
3.  Update the `appsettings.json` or config in the client to point to the `backend_url`.
4.  Run the client. It should appear as "Online" in the Admin Panel (`frontend_url`).
