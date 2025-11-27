terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = var.region
}

# VPC
resource "aws_vpc" "main" {
  cidr_block = "10.0.0.0/16"
  enable_dns_hostnames = true
  tags = { Name = "coredisplay-vpc" }
}

resource "aws_subnet" "public" {
  count             = 2
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.${count.index}.0/24"
  availability_zone = data.aws_availability_zones.available.names[count.index]
  map_public_ip_on_launch = true
  tags = { Name = "coredisplay-public-${count.index}" }
}

resource "aws_subnet" "private" {
  count             = 2
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.${count.index + 10}.0/24"
  availability_zone = data.aws_availability_zones.available.names[count.index]
  tags = { Name = "coredisplay-private-${count.index}" }
}

data "aws_availability_zones" "available" {}

# RDS PostgreSQL
resource "aws_db_subnet_group" "db" {
  name       = "coredisplay-db-subnet"
  subnet_ids = aws_subnet.private[*].id
}

resource "aws_db_instance" "postgres" {
  identifier           = "coredisplay-db"
  engine               = "postgres"
  engine_version       = "16.1"
  instance_class       = "db.t3.micro"
  allocated_storage    = 20
  db_name              = "core_display"
  username             = var.db_username
  password             = var.db_password
  db_subnet_group_name = aws_db_subnet_group.db.name
  skip_final_snapshot  = true
  publicly_accessible  = false
}

# ElastiCache Redis
resource "aws_elasticache_subnet_group" "redis" {
  name       = "coredisplay-redis-subnet"
  subnet_ids = aws_subnet.private[*].id
}

resource "aws_elasticache_cluster" "redis" {
  cluster_id           = "coredisplay-redis"
  engine               = "redis"
  node_type            = "cache.t3.micro"
  num_cache_nodes      = 1
  parameter_group_name = "default.redis7"
  subnet_group_name    = aws_elasticache_subnet_group.redis.name
  port                 = 6379
}

# S3 Bucket
resource "aws_s3_bucket" "media" {
  bucket = "coredisplay-media-${var.environment}"
}

# ECS Cluster
resource "aws_ecs_cluster" "main" {
  name = "coredisplay-cluster"
}

# ECS Task Definition (Backend)
resource "aws_ecs_task_definition" "backend" {
  family                   = "coredisplay-backend"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = 256
  memory                   = 512
  execution_role_arn       = aws_iam_role.ecs_execution_role.arn

  container_definitions = jsonencode([
    {
      name  = "backend"
      image = var.backend_image
      portMappings = [{ containerPort = 8080 }]
      environment = [
        { name = "ConnectionStrings__DefaultConnection", value = "Host=${aws_db_instance.postgres.address};Database=core_display;Username=${var.db_username};Password=${var.db_password}" },
        { name = "Redis__ConnectionString", value = "${aws_elasticache_cluster.redis.cache_nodes[0].address}:6379" }
      ]
    }
  ])
}

# IAM Role (Simplified)
resource "aws_iam_role" "ecs_execution_role" {
  name = "coredisplay-ecs-execution-role"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{ Action = "sts:AssumeRole", Effect = "Allow", Principal = { Service = "ecs-tasks.amazonaws.com" } }]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_execution_role_policy" {
  role       = aws_iam_role.ecs_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}
