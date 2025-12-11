# GPS Triangulation API

A production-ready GPS triangulation service built with .NET 10.0 Minimal APIs. This service provides high-precision geodesic calculations using the Vincenty formula for WGS-84 coordinates.

## Features

- 🌍 **High-Precision Geodesic Calculations**: Uses Vincenty formula for accurate distance calculations
- 🔐 **API Key Authentication**: Secure endpoints with configurable API keys
- 📊 **Rate Limiting**: Built-in IP-based rate limiting
- 🔍 **Distributed Tracing**: OpenTelemetry integration with correlation IDs
- 📝 **Structured Logging**: Serilog with file and console output
- 🚀 **Performance Optimized**: Output caching and response compression
- 🐳 **Docker Ready**: Multi-stage Dockerfile with health checks
- ✅ **Comprehensive Tests**: Unit tests with xUnit and FluentAssertions
- 📖 **OpenAPI/Swagger**: Interactive API documentation

## API Endpoints

### GPS Triangulation
```
POST /api/GpsTriangulate
```
Finds the closest comparison points for each base GPS location within a specified maximum distance.

### Distance Between Points
```
POST /api/DistanceBetweenPoints
```
Calculates the precise distance between two GPS coordinates in feet.

### Health Check
```
GET /health
```
Service health status endpoint.

## Getting Started

### Prerequisites

- .NET 10.0 SDK
- Docker (optional)

### Running Locally

1. **Clone the repository**
   ```powershell
   git clone https://github.com/jimleeii/GpsTriangulation.git
   cd GpsTriangulation
   ```

2. **Restore dependencies**
   ```powershell
   dotnet restore
   ```

3. **Build the project**
   ```powershell
   dotnet build
   ```

4. **Run the application**
   ```powershell
   cd src
   dotnet run
   ```

5. **Access the API**
   - HTTP: http://localhost:5243
   - HTTPS: https://localhost:7145
   - Swagger UI: https://localhost:7145/swagger

### Running with Docker

1. **Build the Docker image**
   ```powershell
   docker build -t gpstriangulation:latest .
   ```

2. **Run with Docker Compose**
   ```powershell
   docker-compose up -d
   ```

3. **Access the API**
   - API: http://localhost:8080
   - Health: http://localhost:8080/health

## Configuration

### API Keys

Configure API keys via environment variable (recommended for production):
```powershell
$env:API_KEYS="your-key-1,your-key-2,your-key-3"
```

Or in `appsettings.json`:
```json
{
  "Authentication": {
    "ApiKeys": [
      "your-api-key-here"
    ]
  }
}
```

### Rate Limiting

Configure in `appsettings.json`:
```json
{
  "IpRateLimiting": {
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 60
      }
    ]
  }
}
```

### CORS

Configure allowed origins in `appsettings.json`:
```json
{
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "https://yourdomain.com"
    ]
  }
}
```

## Usage Examples

### Triangulate GPS Points

```powershell
$headers = @{
    "X-API-Key" = "your-api-key-here"
    "Content-Type" = "application/json"
}

$body = @{
    baseData = @(
        @{ id = 1; lat = 37.7749; lon = -122.4194 }
        @{ id = 2; lat = 37.7750; lon = -122.4193 }
    )
    comparisonData = @(
        @{ station_id = "STA-100"; lat = 37.77491; lon = -122.41939 }
        @{ station_id = "STA-200"; lat = 37.7750; lon = -122.4195 }
    )
    baseLatColumn = "lat"
    baseLonColumn = "lon"
    targetLatColumn = "lat"
    targetLonColumn = "lon"
    maxDistance = 15.0
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7145/api/GpsTriangulate" `
    -Method POST `
    -Headers $headers `
    -Body $body
```

### Calculate Distance Between Two Points

```powershell
$headers = @{
    "X-API-Key" = "your-api-key-here"
    "Content-Type" = "application/json"
}

$body = @{
    point1 = @{ latitude = 37.7749; longitude = -122.4194 }
    point2 = @{ latitude = 34.0522; longitude = -118.2437 }
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7145/api/DistanceBetweenPoints" `
    -Method POST `
    -Headers $headers `
    -Body $body
```

## Running Tests

```powershell
dotnet test
```

With coverage:
```powershell
dotnet test --collect:"XPlat Code Coverage"
```

## Project Structure

```
GpsTriangulation/
├── src/
│   ├── DataContracts/          # Request/Response DTOs
│   ├── EndpointDefinitions/    # Minimal API endpoints
│   ├── Extensions/             # Extension methods
│   ├── Middleware/             # Custom middleware
│   ├── Models/                 # Domain models
│   ├── Services/               # Business logic
│   └── Program.cs              # Application entry point
├── tests/
│   └── GpsTriangulation.Tests/ # Unit tests
├── Dockerfile                  # Docker build configuration
├── docker-compose.yml          # Docker Compose configuration
└── README.md
```

## Production Considerations

### Security
- ✅ API Key authentication implemented
- ✅ Rate limiting configured
- ✅ CORS protection enabled
- ✅ HTTPS enforcement in production
- ⚠️ Consider adding OAuth2/JWT for multi-tenant scenarios
- ⚠️ Store API keys in Azure Key Vault or similar

### Monitoring & Logging
- ✅ Structured logging with Serilog
- ✅ Correlation IDs for request tracing
- ✅ OpenTelemetry integration
- ⚠️ Add Application Insights or equivalent APM
- ⚠️ Configure log retention policies

### Performance
- ✅ Output caching enabled
- ✅ Response compression enabled
- ✅ Request size limits configured
- ⚠️ Consider spatial indexing for large datasets
- ⚠️ Implement database caching if persistence needed

### Reliability
- ✅ Global exception handling
- ✅ Health checks configured
- ✅ Graceful shutdown handling
- ⚠️ Add retry policies (Polly)
- ⚠️ Implement circuit breakers

## Technology Stack

- **Runtime**: .NET 10.0
- **API Framework**: ASP.NET Core Minimal APIs
- **Logging**: Serilog
- **Telemetry**: OpenTelemetry
- **Authentication**: API Key (custom handler)
- **Rate Limiting**: AspNetCoreRateLimit
- **API Versioning**: Microsoft.AspNetCore.Mvc.Versioning
- **Testing**: xUnit, FluentAssertions, Moq
- **Containerization**: Docker

## License

MIT

## Author

Wei Li

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

For issues and questions, please use the GitHub issue tracker.
