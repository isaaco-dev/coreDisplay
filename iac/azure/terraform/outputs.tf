output "vm_public_ip" {
  value = azurerm_public_ip.vm_pip.ip_address
}

output "vm_username" {
  value = var.admin_username
}

output "vm_password" {
  value     = local.admin_password
  sensitive = true
}

output "db_connection_string" {
  value     = "Host=${azurerm_postgresql_flexible_server.postgres.fqdn};Port=5432;Database=core_display;Username=${var.admin_username};Password=${local.admin_password};SSL Mode=Require;Trust Server Certificate=true"
  sensitive = true
}

output "backend_url" {
  value = "https://${azurerm_container_app.backend.latest_revision_fqdn}"
}

output "frontend_url" {
  value = "https://${azurerm_container_app.frontend.latest_revision_fqdn}"
}
