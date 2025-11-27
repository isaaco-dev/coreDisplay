output "backend_url" {
  value = google_cloud_run_v2_service.backend.uri
}

output "db_connection_name" {
  value = google_sql_database_instance.postgres.connection_name
}

output "bucket_name" {
  value = google_storage_bucket.media.name
}
