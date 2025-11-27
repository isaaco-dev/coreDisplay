terraform {
  required_providers {
    google = {
      source  = "hashicorp/google"
      version = "~> 5.0"
    }
  }
}

provider "google" {
  project = var.project_id
  region  = var.region
}

# Cloud SQL (PostgreSQL)
resource "google_sql_database_instance" "postgres" {
  name             = "coredisplay-db"
  database_version = "POSTGRES_15"
  region           = var.region
  deletion_protection = false

  settings {
    tier = "db-f1-micro"
  }
}

resource "google_sql_database" "database" {
  name     = "core_display"
  instance = google_sql_database_instance.postgres.name
}

resource "google_sql_user" "users" {
  name     = var.db_username
  instance = google_sql_database_instance.postgres.name
  password = var.db_password
}

# Cloud Storage
resource "google_storage_bucket" "media" {
  name     = "coredisplay-media-${var.project_id}"
  location = var.region
}

# Cloud Run (Backend)
resource "google_cloud_run_v2_service" "backend" {
  name     = "coredisplay-backend"
  location = var.region
  ingress = "INGRESS_TRAFFIC_ALL"

  template {
    containers {
      image = var.backend_image
      ports {
        container_port = 8080
      }
      env {
        name  = "ConnectionStrings__DefaultConnection"
        value = "Host=/cloudsql/${google_sql_database_instance.postgres.connection_name};Database=core_display;Username=${var.db_username};Password=${var.db_password}"
      }
    }
  }
}

# Public Access for Cloud Run
resource "google_cloud_run_service_iam_member" "public_access" {
  service  = google_cloud_run_v2_service.backend.name
  location = google_cloud_run_v2_service.backend.location
  role     = "roles/run.invoker"
  member   = "allUsers"
}
