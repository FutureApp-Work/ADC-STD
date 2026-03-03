# ADC-STD

## Overview

ADC-STD is a .NET 8.0 application deployed on spark-vm.futureapp.work.

## Deployment

This repository is configured with GitHub Actions for automatic deployment.

### Automatic Deployment

- **Production deployment** is triggered automatically on every push to the `main` branch
- **CI checks** run on every Pull Request to `main`

### Required Secrets

The following secrets must be configured in the GitHub repository settings (`Settings > Secrets and variables > Actions`):

| Secret | Description |
|--------|-------------|
| `SSH_PRIVATE_KEY` | SSH private key for accessing the server |
| `SSH_USER` | SSH username for the server |

### Server Configuration

The application is deployed to `spark-vm.futureapp.work` at `/opt/adc-std`:

- Uses Docker Compose for containerization
- Automatically rebuilds and restarts on deployment
- Health check runs after deployment

### Manual Deployment

To trigger a manual deployment:

1. Go to **Actions > Deploy to Production**
2. Click **Run workflow**
3. Select the branch and click **Run workflow**

### GitHub Actions Workflows

| Workflow | Trigger | Purpose |
|----------|---------|---------|
| `deploy.yml` | Push to `main`, Manual | Build, test, and deploy to production |
| `ci.yml` | Pull Request to `main` | Build and test on PR |

## Development

### Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose (for local testing)

### Building Locally

```bash
# Restore dependencies
dotnet restore

# Build in Release mode
dotnet build -c Release

# Run tests
dotnet test --verbosity normal
```

## License

Private - For FutureApp-Work organization use only.
