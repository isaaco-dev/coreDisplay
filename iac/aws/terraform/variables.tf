variable "region" {
  default = "us-east-1"
}

variable "environment" {
  default = "dev"
}

variable "db_username" {
  default = "coreadmin"
}

variable "db_password" {
  sensitive = true
}

variable "backend_image" {
  default = "coredisplay/backend:latest"
}
