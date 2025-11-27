# Guida al Deployment - CoreDisplay Fleet Platform

Questa guida tecnica copre il deployment su **Locale (Docker)** e **Multi-Cloud (Azure, AWS, GCP)** tramite Terraform.

---

## 1. Sviluppo Locale (Docker Compose) 🐳

Ideale per sviluppo e test rapidi.

### Comandi
```bash
# Avvio Stack
docker-compose up -d --build

# Stop Stack
docker-compose down
```

### Accesso
*   **Admin Panel**: [http://localhost:3000](http://localhost:3000)
*   **API**: [http://localhost:5000](http://localhost:5000)

---

## 2. Cloud Deployment (Infrastructure as Code) ☁️

Scegli il tuo Cloud Provider preferito. Ogni cartella `iac` contiene configurazioni Terraform pronte all'uso.

### A. Microsoft Azure (Consigliato)

Stack: **Container Apps, PostgreSQL Flexible, Redis, Storage Account**.

1.  **Init**: `cd iac/azure/terraform && terraform init`
2.  **Apply**: `terraform apply`
3.  **Output**: Recupera `backend_url` e credenziali VM Windows.

### B. Amazon Web Services (AWS)

Stack: **ECS Fargate, RDS PostgreSQL, ElastiCache Redis, S3**.

1.  **Prerequisiti**: AWS CLI configurata (`aws configure`).
2.  **Init**: `cd iac/aws/terraform && terraform init`
3.  **Apply**: `terraform apply`
    *   *Nota*: Richiede la creazione manuale di un `terraform.tfvars` con `db_password`.

### C. Google Cloud Platform (GCP)

Stack: **Cloud Run, Cloud SQL, Cloud Storage**.

1.  **Prerequisiti**: gcloud CLI autenticata e progetto creato.
2.  **Init**: `cd iac/gcp/terraform && terraform init`
3.  **Apply**: `terraform apply -var="project_id=IL_TUO_PROJECT_ID"`

---

## 3. Configurazione Client Windows

Indipendentemente dal cloud, la procedura per il client è identica:

1.  **Ottieni l'URL Backend**: Dall'output di Terraform (`backend_url` o `rds_endpoint` per DB diretti).
2.  **Configura**: Aggiorna `appsettings.json` nel client Windows.
3.  **Avvia**: Esegui l'eseguibile `CoreDisplay.Windows.exe`.
