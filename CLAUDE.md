# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Domain-Driven Design (DDD) based ASP.NET Core 10.0 Web API. The codebase is organized into three main projects:

- **CompanyName.DDD.API**: Web API layer with controllers, middleware, and configuration
- **CompanyName.DDD.Domain**: Core domain logic organized by aggregates (Product, Sale, TaxType, Kardex, ProductStock)
- **CompanyName.DDD.Test**: Unit and integration tests using MSTest

## Architecture Pattern

The project follows a DDD structure with distinct layers:

- **Aggregates** (Domain): Entity root clusters (ProductAggregate, SaleAggregate, etc.)
- **Application Layer**: Use cases and business logic orchestration
  - `Interfaces/`: Service contracts
  - `DTO/`: Data Transfer Objects (Commands and Responses)
  - `Services/`: Aggregate implementations
- **Infrastructure Layer**: Data access and external dependencies
  - `SQLServer`: Repository implementations using EF Core
  - `Cache/`: In-memory and Redis caching abstractions
- **Domain Layer**: Core entities and business rules
- **Common**: Shared infrastructure (EF Core Context, logging, caching)

Controllers inject aggregates (not repositories directly) and orchestrate domain operations.

## Build & Run

```powershell
# Build the solution
dotnet build CompanyName.DDD.API.sln

# Run the API (from CompanyName.DDD.API directory)
dotnet run

# Run tests
dotnet test CompanyName.DDD.Test.csproj

# Run specific test class
dotnet test CompanyName.DDD.Test.csproj --filter "ClassName=SaleTest"
```

## Key Configuration

**Authentication**: JWT Bearer via Keycloak
- Authority: `http://localhost:8080/realms/IntelicaLocal` (configurable in appsettings)
- Audience: `account`

**Database**: SQL Server
- Connection string in `appsettings/appsettings.json`
- EF Core Context in `CompanyName.DDD.Domain/Common/EFCore/Context.cs`
- Model configurations in `CompanyName.DDD.Domain/Common/EFCore/Configuration/`

**Caching**: Abstractions in `Common/Cache/`
- Default: In-memory cache (`InMemoryCache`)
- Alternative: Redis cache (`RedisCache`)
- Registered as transient via DIExtension

**Environment Detection**: Uses `ASPNETCORE_ENVIRONMENT`
- Swagger UI only available in "Local" environment

## Adding a New Aggregate

1. Create folder in `CompanyName.DDD.Domain/{AggregateNameAggregate}/`
2. Create subdirectories: `Domain/`, `Application/`, `Application/Interfaces/`, `Application/DTO/`, `Infrastructure/SQLServer/`
3. Define domain entity in `Domain/` folder
4. Create aggregate interface and class in `Application/`
5. Create repository interface and implementation in `Application/Interfaces/` and `Infrastructure/`
6. Add EF Core configuration to `Common/EFCore/Configuration/`
7. Register in `DIExtension.AddConfigurations()` in the API project
8. Create controller in `CompanyName.DDD.API/Controllers/`

## Dependency Injection

Services are registered in `CompanyName.DDD.API/Extensions/DIExtension.cs` as transient. The following patterns are used:

- **Aggregates**: Registered as both interface and implementation
- **Repositories**: Registered to their implementations
- **Cross-cutting concerns**: Cache and Logging registered globally

Example registration:
```csharp
services.AddTransient<IProductAggregate, ProductAggregate>();
services.AddTransient<ProductRepository, ProductRepositorySQLServer>();
```

## Common Patterns

**Caching in Aggregates**: Services check cache before calling repository
```csharp
var key = $"{param1}-{param2}-MethodName";
var response = cache.Get<T>(key);
if (response == null) {
    response = repository.Method(params);
    cache.Set(response, key);
}
return response;
```

**Entity Framework Configurations**: Use `IEntityTypeConfiguration<T>` pattern
```csharp
modelBuilder.ApplyConfiguration(new ProductConfiguration());
```

**Controller Method Attributes**:
- `[Authorize]`: Required by default; use `[AllowAnonymous]` for public endpoints
- `[HttpGet/Post/Put/Delete]`: RESTful verb mapping
- `[Route("path")]`: Custom routing

## Development Notes

- Controllers are minimal and delegate to aggregates
- Avoid dependency injection of repositories directly into controllers
- Cache keys should include all relevant parameters for invalidation safety
- The `ExceptionMiddleware` handles cross-cutting error responses
- CORS is configured to allow all origins in "local" policy for development
- Connection retry is configured (5 attempts, 5-second delay) for SQL Server resilience

## File Organization Quick Reference

```
CompanyName.DDD.API/
├── Controllers/               # Minimal controllers by domain area
├── Extensions/               # DIExtension, OptionsExtension
├── Middleware/               # ExceptionMiddleware
├── appsettings/             # Configuration files
└── Properties/              # Launch settings

CompanyName.DDD.Domain/
├── {AggregateRoot}Aggregate/
│   ├── Domain/              # Entities
│   ├── Application/         # Aggregate service + DTO
│   ├── Application/Interfaces/  # Service & Repository contracts
│   └── Infrastructure/      # Repository implementations
└── Common/
    ├── EFCore/              # Context + configurations
    ├── Cache/               # Caching abstractions
    └── Log/                 # Logging abstractions
```

## Auditing (mandatory)

- Every persisted aggregate entity inherits from `CompanyName.DDD.Domain.Common.Audit.AuditableEntity` (`CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`).
- Do not assign audit fields by hand: `Context.SaveChanges` fills them using `ICurrentUserService`.
- A new entity needs its columns in the database: add an idempotent script under `CompanyName.DDD.API/Scripts/` (see `001_audit_columns.sql`).
- Secrets (the connection string) are set with `ConnectionStrings__Operations`; never in `appsettings`.
