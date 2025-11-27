resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

# Random Password Generation
resource "random_password" "password" {
  length           = 16
  special          = true
  override_special = "!#$%&*()-_=+[]{}<>:?"
}

locals {
  admin_password = var.admin_password != null ? var.admin_password : random_password.password.result
}

# Networking
resource "azurerm_virtual_network" "vnet" {
  name                = "vnet-coredisplay-dev"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  address_space       = ["10.0.0.0/16"]
}

resource "azurerm_subnet" "snet_app" {
  name                 = "snet-app"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.1.0/24"]
}

resource "azurerm_subnet" "snet_vm" {
  name                 = "snet-vm"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.2.0/24"]
}

# PostgreSQL Flexible Server
resource "azurerm_postgresql_flexible_server" "postgres" {
  name                   = "db-coredisplay-${random_string.suffix.result}"
  resource_group_name    = azurerm_resource_group.rg.name
  location               = azurerm_resource_group.rg.location
  version                = "16"
  administrator_login    = var.admin_username
  administrator_password = local.admin_password
  storage_mb             = 32768
  sku_name               = "B_Standard_B1ms"
  zone                   = "1"
}

resource "azurerm_postgresql_flexible_server_database" "db" {
  name      = "core_display"
  server_id = azurerm_postgresql_flexible_server.postgres.id
  collation = "en_US.utf8"
  charset   = "utf8"
}

resource "azurerm_postgresql_flexible_server_firewall_rule" "allow_azure_services" {
  name             = "allow-azure-services"
  server_id        = azurerm_postgresql_flexible_server.postgres.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

# Redis Cache
resource "azurerm_redis_cache" "redis" {
  name                = "cache-coredisplay-${random_string.suffix.result}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  capacity            = 0
  family              = "C"
  sku_name            = "Basic"
  enable_non_ssl_port = false
  minimum_tls_version = "1.2"
}

# Storage Account
resource "random_string" "suffix" {
  length  = 6
  special = false
  upper   = false
}

resource "azurerm_storage_account" "storage" {
  name                     = "stcoredisplay${random_string.suffix.result}"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

# Container Apps Environment
resource "azurerm_container_app_environment" "env" {
  name                       = "container-env-core"
  location                   = azurerm_resource_group.rg.location
  resource_group_name        = azurerm_resource_group.rg.name
  infrastructure_subnet_id   = azurerm_subnet.snet_app.id
  internal_load_balancer_enabled = false
}

# RabbitMQ Container
resource "azurerm_container_app" "rabbitmq" {
  name                         = "app-rabbitmq"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

  template {
    container {
      name   = "rabbitmq"
      image  = var.rabbitmq_image
      cpu    = 0.5
      memory = "1Gi"
      
      env {
        name  = "RABBITMQ_DEFAULT_USER"
        value = var.admin_username
      }
      env {
        name  = "RABBITMQ_DEFAULT_PASS"
        value = local.admin_password
      }
    }
  }
  
  ingress {
    external_enabled = false
    target_port      = 5672
    transport        = "tcp"
    traffic_weight {
      percentage = 100
      latest_revision = true
    }
  }
}

# Backend API Container
resource "azurerm_container_app" "backend" {
  name                         = "app-backend"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

  template {
    container {
      name   = "backend"
      image  = var.backend_image
      cpu    = 0.5
      memory = "1Gi"

      env {
        name  = "ConnectionStrings__DefaultConnection"
        value = "Host=${azurerm_postgresql_flexible_server.postgres.fqdn};Port=5432;Database=core_display;Username=${var.admin_username};Password=${local.admin_password};SSL Mode=Require;Trust Server Certificate=true"
      }
      env {
        name  = "Redis__ConnectionString"
        value = azurerm_redis_cache.redis.primary_connection_string
      }
      env {
        name  = "RabbitMQ__HostName"
        value = azurerm_container_app.rabbitmq.latest_revision_fqdn # Note: This might need adjustment based on internal DNS resolution in ACA
      }
    }
  }

  ingress {
    external_enabled = true
    target_port      = 8080
    traffic_weight {
      percentage = 100
      latest_revision = true
    }
  }
}

# Frontend Container
resource "azurerm_container_app" "frontend" {
  name                         = "app-frontend"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

  template {
    container {
      name   = "frontend"
      image  = var.frontend_image
      cpu    = 0.25
      memory = "0.5Gi"
      
      env {
        name  = "VITE_API_URL"
        value = "https://${azurerm_container_app.backend.latest_revision_fqdn}"
      }
    }
  }

  ingress {
    external_enabled = true
    target_port      = 80
    traffic_weight {
      percentage = 100
      latest_revision = true
    }
  }
}

# Windows VM for Client Testing
resource "azurerm_public_ip" "vm_pip" {
  name                = "pip-vm-client"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  allocation_method   = "Dynamic"
}

resource "azurerm_network_interface" "vm_nic" {
  name                = "nic-vm-client"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = azurerm_subnet.snet_vm.id
    private_ip_address_allocation = "Dynamic"
    public_ip_address_id          = azurerm_public_ip.vm_pip.id
  }
}

resource "azurerm_network_security_group" "vm_nsg" {
  name                = "nsg-vm-client"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  security_rule {
    name                       = "RDP"
    priority                   = 1000
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "3389"
    source_address_prefix      = "*" # In production, restrict this to specific IP
    destination_address_prefix = "*"
  }
}

resource "azurerm_network_interface_security_group_association" "vm_nsg_assoc" {
  network_interface_id      = azurerm_network_interface.vm_nic.id
  network_security_group_id = azurerm_network_security_group.vm_nsg.id
}

resource "azurerm_windows_virtual_machine" "vm" {
  name                = "vm-client"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  size                = "Standard_B2s"
  admin_username      = var.admin_username
  admin_password      = local.admin_password
  network_interface_ids = [
    azurerm_network_interface.vm_nic.id,
  ]

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = "MicrosoftWindowsDesktop"
    offer     = "Windows-11"
    sku       = "win11-21h2-pro"
    version   = "latest"
  }
}
