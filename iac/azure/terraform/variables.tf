variable "location" {
  description = "The Azure Region in which all resources in this example should be created."
  default     = "East US"
}

variable "resource_group_name" {
  description = "The name of the Resource Group."
  default     = "rg-coredisplay-dev"
}

variable "admin_username" {
  description = "The admin username for the VM and Database."
  default     = "coreadmin"
}

variable "admin_password" {
  description = "The admin password for the VM and Database. If not provided, a random one will be generated."
  type        = string
  sensitive   = true
  default     = null
}

variable "backend_image" {
  description = "Docker image for the backend API."
  default     = "mcr.microsoft.com/azuredocs/aci-helloworld" # Placeholder
}

variable "frontend_image" {
  description = "Docker image for the frontend admin panel."
  default     = "nginx:alpine" # Placeholder
}

variable "rabbitmq_image" {
  description = "Docker image for RabbitMQ."
  default     = "rabbitmq:3-management"
}
