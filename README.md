# TrackManagement

## Prerequisites

- .NET 11 SDK
- A SQL Server instance accessible to the API

The API uses the `ConnectionStrings:Database` setting by default. It can also use separate `ConnectionStrings:WriteDatabase` and `ConnectionStrings:ReadDatabase` values. The Docker Compose configuration starts the API only; provide a separate SQL Server instance. For local development, configure its connection string and the JWT signing key with .NET user secrets rather than putting credentials in source control:

```powershell
dotnet user-secrets set "ConnectionStrings:Database" "Server=localhost,1433;Database=TrackManagementDb;User Id=<your-user>;Password=<your-password>;Encrypt=False;TrustServerCertificate=True" --project src/TrackManagement.API
dotnet user-secrets set "JwtSettings:SecretKey" "<your random secret, at least 32 bytes>" --project src/TrackManagement.API
```

The JWT configuration also requires `JwtSettings:Issuer` and `JwtSettings:Audience`. They have development defaults in configuration; set them explicitly for other environments. Keep connection strings, signing keys, and real credentials private.

## Run the API

From the repository root, restore and run the API with the HTTP launch profile:

```powershell
dotnet run --project src/TrackManagement.API --launch-profile http
```

The API listens at `http://localhost:5062`; Swagger UI is at `http://localhost:5062/swagger`.

On startup, the API calls `Database.MigrateAsync()` and applies any migrations that have not yet been applied, then seeds sample artists, tracks, DSPs, and an admin user. Ensure SQL Server is running and the connection string is valid before starting the API. You normally do not need to run `database update` separately.

## Create and run migrations manually

When the EF Core model changes, install or update the EF CLI tool to version 11.0.0, matching the project's EF Core version:

```powershell
dotnet tool install --global dotnet-ef --version 11.0.0
```

If `dotnet-ef` is already installed, update it instead:

```powershell
dotnet tool update --global dotnet-ef --version 11.0.0
```

From the repository root, create a migration and apply pending migrations with:

```powershell
dotnet ef migrations add <MigrationName> --project src/TrackManagement.Infrastructure --startup-project src/TrackManagement.API --context TrackManagementDbContext --output-dir Migrations
dotnet ef database update --project src/TrackManagement.Infrastructure --startup-project src/TrackManagement.API --context TrackManagementDbContext
```

The API's startup migration remains enabled, so it will also apply the new migration when the API next starts.

## Obtain a JWT

On a new database, the startup seeder creates a development admin account:

- Email: `admin@dev.local`
- Password: `Admin@123!`

Request a token from the login endpoint:

```powershell
$response = Invoke-RestMethod `
  -Method Post `
  -Uri "http://localhost:5062/api/auth/login" `
  -ContentType "application/json" `
  -Body '{"email":"admin@dev.local","password":"Admin@123!"}'

$token = $response.token
$token
```

Send the returned token to protected endpoints using the `Authorization: Bearer <token>` header. The token lifetime is controlled by `JwtSettings:ExpiryMinutes` (60 minutes by default).

**Security:** The current startup seeder creates this default admin account whenever it is missing, regardless of environment. Do not deploy with the default credentials; remove or replace this seeding behavior and configure production secrets before exposing the API.
