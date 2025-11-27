variable "project_id" {
  description = "GCP Project ID"
}

variable "region" {
  default = "us-central1"
}

variable "db_username" {
  default = "coreadmin"
}

variable "db_password" {
  sensitive = true
}

variable "backend_image" {
  default = "gcr.io/google-samples/hello-app:1.0" # Placeholder
}
