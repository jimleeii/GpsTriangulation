# Quick Deployment Guide

## Prerequisites
- .NET 10.0 SDK
- Docker (optional)
- API Key(s) for authentication

## Environment Variables

### Required
```powershell
# Production API Keys (comma-separated)
$env:API_KEYS="key1,key2,key3"
```

### Optional
```powershell
# Environment
$env:ASPNETCORE_ENVIRONMENT="Production"

# Custom URLs
$env:ASPNETCORE_URLS="http://+:8080"

# Allowed Origins (semicolon-separated)
$env:CORS_ALLOWED_ORIGINS="https://yourdomain.com;https://app.yourdomain.com"
```

## Deployment Options

### Option 1: Docker (Recommended)

1. **Build and run with Docker Compose:**
   ```powershell
   # Set API keys
   $env:API_KEYS="your-production-key-here"
   
   # Start services
   docker-compose up -d
   
   # Check logs
   docker-compose logs -f
   
   # Verify health
   curl http://localhost:8080/health
   ```

2. **Stop services:**
   ```powershell
   docker-compose down
   ```

### Option 2: Direct Deployment

1. **Publish the application:**
   ```powershell
   dotnet publish src/GpsTriangulation.csproj -c Release -o ./publish
   ```

2. **Set environment variables:**
   ```powershell
   $env:API_KEYS="your-production-key-here"
   $env:ASPNETCORE_ENVIRONMENT="Production"
   ```

3. **Run the application:**
   ```powershell
   cd publish
   dotnet GpsTriangulation.dll
   ```

### Option 3: Azure App Service

1. **Configure Application Settings:**
   - `API_KEYS`: Your API keys (comma-separated)
   - `ASPNETCORE_ENVIRONMENT`: Production
   - `Cors__AllowedOrigins__0`: https://yourdomain.com
   - `IpRateLimiting__GeneralRules__0__Limit`: 100

2. **Deploy via Azure CLI:**
   ```powershell
   # Login to Azure
   az login
   
   # Publish and deploy
   dotnet publish -c Release
   az webapp deploy --resource-group MyResourceGroup --name MyAppName --src-path ./publish
   ```

### Option 4: Kubernetes

1. **Create ConfigMap:**
   ```yaml
   apiVersion: v1
   kind: ConfigMap
   metadata:
     name: gpstriangulation-config
   data:
     ASPNETCORE_ENVIRONMENT: "Production"
   ```

2. **Create Secret:**
   ```yaml
   apiVersion: v1
   kind: Secret
   metadata:
     name: gpstriangulation-secret
   type: Opaque
   stringData:
     API_KEYS: "your-production-key-here"
   ```

3. **Deploy:**
   ```yaml
   apiVersion: apps/v1
   kind: Deployment
   metadata:
     name: gpstriangulation
   spec:
     replicas: 3
     selector:
       matchLabels:
         app: gpstriangulation
     template:
       metadata:
         labels:
           app: gpstriangulation
       spec:
         containers:
         - name: api
           image: gpstriangulation:latest
           ports:
           - containerPort: 8080
           envFrom:
           - configMapRef:
               name: gpstriangulation-config
           - secretRef:
               name: gpstriangulation-secret
           livenessProbe:
             httpGet:
               path: /health
               port: 8080
             initialDelaySeconds: 10
             periodSeconds: 30
   ```

## Post-Deployment Checklist

### Security
- [ ] API keys changed from defaults
- [ ] CORS origins configured for production domains
- [ ] HTTPS enforced
- [ ] Rate limiting verified
- [ ] Security headers configured (if using reverse proxy)

### Monitoring
- [ ] Logs accessible and rotating properly
- [ ] Health endpoint responding
- [ ] Application Insights configured (if using Azure)
- [ ] Alerts configured for errors/downtime

### Performance
- [ ] Response caching working
- [ ] Compression enabled
- [ ] Rate limits appropriate for load
- [ ] Connection pooling configured

### Testing
- [ ] Health check passes: `GET /health`
- [ ] Swagger accessible (dev only): `GET /swagger`
- [ ] API key authentication working
- [ ] Sample API call succeeds

## Sample API Test

```powershell
# Test with PowerShell
$headers = @{
    "X-API-Key" = "your-api-key-here"
    "Content-Type" = "application/json"
}

$body = @{
    point1 = @{ latitude = 37.7749; longitude = -122.4194 }
    point2 = @{ latitude = 34.0522; longitude = -118.2437 }
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://your-domain.com/api/DistanceBetweenPoints" `
    -Method POST `
    -Headers $headers `
    -Body $body
```

Expected response: Distance in feet (~1,832,556 feet for SF to LA)

## Monitoring Endpoints

- **Health Check**: `GET /health`
- **Metrics**: Check logs at `/app/logs/` (Docker) or configured path
- **Correlation IDs**: Check `X-Correlation-Id` header in responses

## Troubleshooting

### Issue: 401 Unauthorized
- Verify API key is set correctly
- Check `X-API-Key` header is included
- Confirm API_KEYS environment variable is set

### Issue: 429 Too Many Requests
- Rate limit exceeded
- Wait for rate limit window to reset
- Adjust `IpRateLimiting:GeneralRules` in config

### Issue: Health check failing
- Check application logs in `/app/logs/`
- Verify port 8080 is accessible
- Check container is running: `docker ps`

### Issue: CORS errors
- Add your domain to `Cors:AllowedOrigins`
- Restart application after config changes
- Verify preflight requests are allowed

## Rollback Procedure

### Docker
```powershell
# Stop current version
docker-compose down

# Rollback to previous image
docker tag gpstriangulation:previous gpstriangulation:latest
docker-compose up -d
```

### Azure App Service
```powershell
# Swap deployment slots
az webapp deployment slot swap --resource-group MyResourceGroup --name MyAppName --slot staging --target-slot production
```

## Support

For issues, check:
1. Application logs (`/app/logs/` or configured path)
2. Container logs (`docker-compose logs`)
3. Health endpoint status
4. GitHub Issues: https://github.com/jimleeii/GpsTriangulation/issues
