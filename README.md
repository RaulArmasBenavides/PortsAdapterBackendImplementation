# CompanyName.DDD — sales, products and tax API

ASP.NET Core 10 API with Domain-Driven Design by aggregate (Product, Sale, TaxType, ProductStock, Kardex). The database is SQL Server.

## Structure

```
CompanyName.DDD.API/
├── CompanyName.DDD.API/         # Web layer: controllers, middleware, DI, Program.cs
├── CompanyName.DDD.Domain/      # Domain and application code per aggregate (see below)
│   ├── Common/                  # EF Core (Context + configurations), Audit, Cache, Log
│   └── <Aggregate>Aggregate/
│       ├── Domain/              # Entities and business rules
│       ├── Application/         # Use cases (services), DTOs, repository and service interfaces
│       └── Infrastructure/      # SQL Server repositories
└── CompanyName.DDD.Test/        # MSTest tests
```

Controllers use aggregates (`ISaleAggregate`, `IProductAggregate`, ...), never repositories directly.

## Requirements

- .NET SDK 10
- SQL Server with the `Operations` database

## Configuration

Secrets do not go in `appsettings/appsettings.json`. Set them with environment variables or User Secrets:

| Variable | Description |
|---|---|
| `ConnectionStrings__Operations` | SQL Server connection string (required) |

Other values (`Auth:Authority`, `Auth:Audience`) are in `appsettings.json` and point to the Keycloak realm.

If the connection string is missing, the application does not start and reports the variable to set. Environment variables override the JSON file.

## Run

```bash
cd CompanyName.DDD.API/CompanyName.DDD.API
export ConnectionStrings__Operations="Server=...;Database=Operations;..."
dotnet run
```

Swagger is available only when `ASPNETCORE_ENVIRONMENT=Local`.

## Database and auditing

The `Kardex`, `Product`, `ProductStock`, `Sale`, `SaleDetail` and `TaxType` tables have these audit columns:

| Column | Type | Description |
|---|---|---|
| `CreatedAt` | `datetime2` | UTC creation date |
| `CreatedBy` | `nvarchar(100)` | User who created the record (`preferred_username` from the JWT) |
| `UpdatedAt` | `datetime2` | UTC date of the last change |
| `UpdatedBy` | `nvarchar(100)` | User who last changed the record |

The values are filled automatically in `Context.SaveChanges` (base class `AuditableEntity`). On update, `CreatedAt` and `CreatedBy` are never overwritten.

The project has no EF migrations: the database owns the schema. To add auditing, run the idempotent script:

```
CompanyName.DDD.API/Scripts/001_audit_columns.sql
```

The `CreationUser` and `SaleDate` columns of `Sale` predate auditing and are kept for compatibility.

## Endpoints

All endpoints require a JWT (global `RequireAuthorization`).

| Method | Route | Description |
|---|---|---|
| POST | `/Sale` | Records a sale with its details and validates totals |
| GET | `/Product/GetBySaleFilter/{productName}/{taxDescriprion}` | Active products for sale; `-` skips the filter |
| GET | `/TaxType` | Tax types |

## Tests

```bash
dotnet test CompanyName.DDD.API/CompanyName.DDD.Test/CompanyName.DDD.Test.csproj
```

Covers detail-total validation and auditing tests on EF Core InMemory.

## Docker

```bash
docker build -f dockerfile -t ports-adapter-api .
docker run -p 8080:8080 -e ConnectionStrings__Operations="..." ports-adapter-api
```

## Pending

- Replace the console logger (`StandartOuputLog`) with a structured logger.
- Review the `Sale`/`SaleDetail` naming against `CreationUser` when the data migration is planned.
