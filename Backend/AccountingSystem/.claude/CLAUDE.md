# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Critical conventions

- **Code comments must be written in professional English only. Do not use emojis in comments, commit messages, or any code artifact.** This applies to XML doc comments (`///`), inline `//` comments, `<summary>` blocks, log messages, and exception messages.
- Do not translate identifiers, enum members, or API contracts into Hebrew — the codebase is English-only at the code level even though the product is Hebrew-facing.

## Architecture diagram

```
                        ┌─────────────────────────────┐
                        │   AccountingSystem.API      │
                        │   (ASP.NET Core 9)          │
                        │                             │
                        │  Controllers ──► IMediator  │
                        │  Program.cs (composition)   │
                        └──────────────┬──────────────┘
                                       │
                       references all three projects
                                       │
                ┌──────────────────────┼──────────────────────┐
                ▼                      ▼                      ▼
   ┌─────────────────────┐  ┌─────────────────────┐  ┌─────────────────────┐
   │   Application       │  │   Infrastructure    │  │      Domain         │
   │   (CQRS)            │  │   (EF Core / Auth)  │  │   (Pure model)      │
   │                     │  │                     │  │                     │
   │  Commands           │  │  AccountingDbContext│  │  Entities           │
   │  Queries            │◄─┤  Repositories       │  │  Enums              │
   │  Handlers (MediatR) │  │  UnitOfWork         │  │  Repository ifaces  │
   │  Validators (Fluent)│  │  JwtTokenService    │  │                     │
   │  DTOs + AutoMapper  │  │  Hangfire Jobs      │  │  (no dependencies)  │
   └──────────┬──────────┘  └──────────┬──────────┘  └──────────▲──────────┘
              │                        │                        │
              └────── references ──────┴────────────────────────┘

   Request flow:
     HTTP ──► Controller ──► IMediator.Send(Command/Query)
          ──► Handler (Application) ──► Repository (Infrastructure)
          ──► AccountingDbContext ──► PostgreSQL
          ──► Entity ──► AutoMapper ──► DTO ──► JSON response
```

## Solution layout

.NET 9 Clean Architecture solution at `Backend/AccountingSystem/AccountingSystem.sln` with 4 projects under `src/`:

- **AccountingSystem.Domain** — entities, enums, repository interfaces. No external dependencies.
- **AccountingSystem.Application** — MediatR Commands/Queries/Handlers, DTOs, FluentValidation validators, AutoMapper `MappingProfile`. References Domain only.
- **AccountingSystem.Infrastructure** — EF Core `AccountingDbContext`, generic + per-entity repositories, `UnitOfWork`, `JwtTokenService`, `AuthenticationService`, `CurrentUserService`, Hangfire jobs. References Domain + Application.
- **AccountingSystem.API** — ASP.NET Core controllers under `Controller/`, `Program.cs` composition root. References all three.

Dependency direction is strictly inward: Domain has no references; Infrastructure and API depend on Application + Domain.

## Common commands

Run all `dotnet` commands from `Backend/AccountingSystem/src/AccountingSystem.API/` unless noted.

```powershell
# Run API (HTTP only on :5096)
dotnet run

# Run API with HTTPS (:7118) + HTTP (:5096)
dotnet run --launch-profile https

# Build entire solution
dotnet build ../../AccountingSystem.sln

# EF Core migrations - run from the API project, target Infrastructure
dotnet ef migrations add <Name> --project ../AccountingSystem.Infrastructure
dotnet ef database update --project ../AccountingSystem.Infrastructure
```

Swagger UI is served at `/swagger` in Development. Hangfire dashboard is at `/hangfire`.

No test project currently exists in the solution.

## Configuration & secrets

`Program.cs` loads a `.env` file from the project root at startup. Environment variables override `appsettings.json`. Required keys are documented in `Backend/AccountingSystem/.env.example`:

- `DB_CONNECTION_STRING` — PostgreSQL (Npgsql)
- `JWT_SECRET_KEY` (≥32 chars), `JWT_ISSUER`, `JWT_AUDIENCE`
- `GOOGLE_CLIENT_ID`, `GOOGLE_CLIENT_SECRET` — OAuth login

The dev DB is `Irgunit2`; production uses `Irgunit3` (per `appsettings.json` / `appsettings.Production.json`).

## Architecture conventions

**CQRS via MediatR.** Controllers are thin: they construct a Command or Query DTO and call `_mediator.Send(...)`. New endpoints should follow this pattern — do not add business logic in controllers, and do not call repositories directly from controllers.

- Commands live in `Application/Commands/`, Queries in `Application/Queries/`, handlers in `Application/Handlers/`.
- Each command/query typically has a matching FluentValidation validator in `Application/Validators/`.
- Cross-entity DTO mapping goes through `Application/Mappings/MappingProfile.cs` (AutoMapper).

**Persistence.** `AccountingDbContext` is a `partial class` — EF model configuration is split across files. PostgreSQL enums are registered in `Program.cs` via `MapEnum<T>` for `TaskStatus1`, `ReportStatus`, `PaymentMethod`, `TaskCategory`, `TaskPriority`, etc. When adding a new enum-backed column, register the mapping there or migrations will fail.

Repositories follow a generic-base + specific-derived pattern; transactions are coordinated through `IUnitOfWork`. Prefer adding a method to an existing repository over bypassing it with raw `DbContext` access.

**Read-only views** (`VwActiveTasks`, `VwUpcomingReports`, `VwCompanyDetails`, `VwWorkerCompanies`) are mapped as keyless entities and exposed for reporting queries — don't try to insert/update through them.

**Auth.** `JwtTokenService` issues 7-hour tokens with claims: `NameIdentifier`, `Email`, `Name`, `Role`, `EmployeeId`, `FirmId`. `[Authorize]` is applied per-endpoint, not globally. `CurrentUserService` extracts the acting user from `HttpContext` — use it instead of re-parsing claims.

**CORS.** Only `http(s)://localhost:4200` (the Angular frontend in `Fronted/Angular-app/`) is allowed, with credentials. If you change the frontend origin, update the `AllowAngular` policy in `Program.cs`.

## Background jobs (Hangfire)

Registered in `Program.cs`, backed by the same PostgreSQL database. Three jobs:

- **ReportGenerationJob** — recurring, 25th of each month at 01:00 Israel time.
- **CheckReportGenerationJob** — daily at 00:00.
- **AutomaticTaskGenerationJob** — runs as a hosted service.

When changing a recurring schedule, update the `RecurringJob.AddOrUpdate(...)` call in `Program.cs` — the Hangfire DB stores the previous schedule until overwritten.

## Domain at a glance

The system manages an accounting firm's workflow: an `AccountingFirm` has `Workers` (with `Role`) who handle `CompanyTask`s (typed by `TaskType` / `TaskTypeConfiguration`, with `ChecklistItems`) for client `Company` records. Recurring `ReportInstance`s are generated from `CompanyReportConfig` + `ReportType` + `Frequency`, tracked through `ReportStatus` (Pending → Reported → Paid → Approved) and `PaymentMethod`. All mutations write `AuditLog` entries.
