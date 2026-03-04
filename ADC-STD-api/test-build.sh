#!/bin/bash
# Test Docker build locally

set -e

echo "Testing Docker build..."

# Copy docker-compose to parent directory temporarily for build context
cd /home/openclaw/.openclaw/workspace/ADC-STD

# Create a test .env file if not exists
if [ ! -f .env ]; then
    echo "Creating test .env file..."
    echo "DB_ROOT_PASSWORD=testpassword123" > .env
fi

# Build the API
pe "Docker Compose build test..."
docker compose -f ADC-STD-api/docker-compose.yml build api --no-cache

echo "Build completed successfully!"
