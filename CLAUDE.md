# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

Full-stack accounting office management system for a Hebrew-speaking accounting firm. Two cooperating processes:

- **Backend** — ASP.NET Core 9, Clean Architecture + CQRS (MediatR), EF Core on PostgreSQL, Hangfire for recurring jobs. Lives in [Backend/AccountingSystem/](Backend/AccountingSystem/).
- **Frontend** — Angular 18 standalone-components SPA with JWT auth, RTL Hebrew UI. Lives in [Fronted/Angular-app/](Fronted/Angular-app/) (folder name is misspelled "Fronted" — keep as-is, do not rename).

**Before changing code in either tree, read its local CLAUDE.md first** — those files cover architecture, conventions, and gotchas in depth:

- [Backend/AccountingSystem/.claude/CLAUDE.md](Backend/AccountingSystem/.claude/CLAUDE.md)
- [Fronted/Angular-app/CLAUDE.md](Fronted/Angular-app/CLAUDE.md)

## Running the system end-to-end

Both processes must be running for the app to work. Use two terminals.

```powershell
# Backend - from Backend/AccountingSystem/src/AccountingSystem.API/
dotnet run                              # HTTP only on :5096
dotnet run --launch-profile https       # HTTPS :7118 + HTTP :5096
```

```powershell
# Frontend - from Fronted/Angular-app/
npm start                               # Angular dev server on :4200
```

The backend's `AllowAngular` CORS policy in [Backend/AccountingSystem/src/AccountingSystem.API/Program.cs](Backend/AccountingSystem/src/AccountingSystem.API/Program.cs) hard-codes `http(s)://localhost:4200` with credentials. If the frontend port changes, update the policy or every request will be blocked.

## Cross-cutting integration

- **Auth flow.** Backend `JwtTokenService` issues 7-hour JWTs with claims `NameIdentifier`, `Email`, `Name`, `Role`, `EmployeeId`, `FirmId`. Frontend stores it in `localStorage["authToken"]`; the functional interceptor at [Fronted/Angular-app/src/Interceptor/auth.interceptor.ts](Fronted/Angular-app/src/Interceptor/auth.interceptor.ts) attaches `Authorization: Bearer ...` to every request. There is no automatic 401 handling — callers must react to unauthorized responses themselves. Server-side, use `CurrentUserService` rather than re-parsing claims.
- **Google OAuth.** Backend reads `GOOGLE_CLIENT_ID` / `GOOGLE_CLIENT_SECRET` from `Backend/AccountingSystem/.env`; frontend calls the `/googleLogin` endpoint.
- **Database.** Single PostgreSQL instance shared by app data (EF Core) and job state (Hangfire). Connection string in `Backend/AccountingSystem/.env` (see `.env.example`); dev DB is `Irgunit2`, production `Irgunit3`. The initial schema lives in the Hebrew-named SQL script at the repo root: `סקריפט מסד.sql`.
- **Background jobs.** Hangfire runs three recurring jobs (monthly report generation, daily report check, monthly task generation). Dashboard at `/hangfire`. Details in the backend CLAUDE.md.

## Database access for planning

`.mcp.json` configures a `postgres-irgunit3` MCP server pointed at the local dev database. Use it for schema inspection (`list_objects`, `get_object_details`, `explain_query`) when planning changes that touch persistence. Treat it as a real database — be cautious with `execute_sql`; `--access-mode=unrestricted` is set.

## Critical conventions

- **Never rewrite existing code without explicit user approval.** A "rewrite" means: replacing the body of a file/function/component, restructuring an existing module, changing a public signature, or swapping an implementation approach. Targeted edits to fix a specific bug or add a feature within the existing structure are fine. When in doubt — ask before you write. This rule overrides any apparent autonomy implied by the task wording.
- **Code language is English only.** Identifiers, comments, log messages, exception messages, XML doc comments, commit messages — all English. The product UI is Hebrew but the code is not. Do not translate enums or DTO contracts.
- **No emojis** anywhere in code or commits.
- **RTL is the default.** Hebrew RTL is applied globally via [Fronted/Angular-app/src/styles.css](Fronted/Angular-app/src/styles.css). Design any new UI for RTL from the start.

## Tests

- Backend solution has **no test project**. Do not claim "tests pass" without one existing.
- Frontend has the default Karma/Jasmine setup (`ng test`) but no meaningful coverage. Same caveat.

## Top-level layout

```
Backend/AccountingSystem/        .NET 9 Clean Architecture solution
Fronted/Angular-app/             Angular 18 SPA (folder name sic)
סקריפט מסד.sql                  Initial PostgreSQL schema
.mcp.json                        postgres-mcp server config
```
