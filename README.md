# Trip Planner and Outing Platform | TripOut

## This project introduces an intelligent trip planning and guidance system that aims to unify planning and exploration into a single interactive platform.

`TripOut` is a modern modular monolith built with ASP.NET Core that powers travel, chat, notifications, and user services in a single deployable backend.

It follows Clean Architecture principles with separated `Common` and module-specific layers, and it is container-ready with Docker and PostgreSQL.

### Tech Stack

- ASP.NET Core 10 — Web API and application runtime
- PostgreSQL / PostGIS — primary database
- Docker / Docker Compose — containerized local development
- Rebus — integration event routing and in-memory event transport
- FluentValidation — request validation
- Minio — object storage for uploads
- Serilog — structured logging
- SignalR — real-time chat functionality
- Scalar — API documentation and development tooling
---

## Core Features

- Modular Monolith Architecture
  - Independent domain modules for `Users`, `Notifications`, `Trips`, and `Conversations`
  - Shared common services for cross-cutting concerns
  - Single deployment unit with modular organization

- Multi-Module Service Layer
  - `Common.Application` for validators, domain events, mappers, and shared application services
  - `Common.Infrastructure` for event bus, Minio file storage, and database integration
  - `Common.Presentation` for authentication, controllers, JSON settings, and logging

- Real-Time Chat
  - `Modules.Conversations` exposes `ChatHub`
  - Conversation group membership is assigned on connect
  - Supports typing, stop typing, and read receipt notifications

- Integration Events
  - Rebus in-memory transport for lightweight event delivery
  - Subscribes to events like `UserCreatedIntegrationEvent`, `SendMessageIntegrationEvent`, and `SendSystemMessageIntegrationEvent`
  - Decorated handlers with inbox/outbox idempotent patterns

- File Upload Storage
  - Uses Minio for object storage
  - Automatically creates the `uploads` bucket in development
  - Provides file service abstraction through `IFileService`

- Localization
  - Supports English (`en`) and Arabic (`ar`)
  - Configured request localization middleware

- Custom Header-based Authentication
  - Uses `X-User-Id` for identifying the user
  - Optional `X-User-Role` header for role claims
  - Implements a forwarded claims authentication handler

---

## Project Structure

```
trpr.backend/
├── Web/Api/                  # ASP.NET Core API project and startup
├── Common/                   # Shared cross-cutting layers
│   ├── Common.Application/    # Validation, domain events, mappers, correlation
│   ├── Common.Domain/         # Domain abstractions and event contracts
│   ├── Common.Infrastructure/ # Event bus, Minio, PostgreSQL support
│   └── Common.Presentation/   # Authentication, JSON, controllers, logging
└── Modules/
    ├── Users/
    ├── Notifications/
    ├── Trips/
    └── Conversations/
```

Each module exposes its own application, infrastructure, and presentation registration and is wired into startup through `Program.cs`.

---

## Startup Flow

`Web/Api/Program.cs` configures the application as follows:

1. Loads `appsettings.json` and module-specific development settings
2. Adds common cross-cutting services
3. Registers `Users`, `Notifications`, `Trips`, and `Conversations` modules
4. Enables integration event support and localization
5. Configures Serilog for logging
6. Builds the app and enables static files
7. In development:
   - maps OpenAPI / Swagger
   - applies EF Core database migrations
   - ensures Minio bucket creation
8. Maps SignalR hub at `chat-hub`
9. Applies localization, middleware, HTTPS redirection, authentication, authorization, and controller routing
10. Starts the ASP.NET Core application

---

## Docker Setup

This repository includes a `docker-compose.yaml` file with:

- `trpr.database` — PostgreSQL / PostGIS database
- `trpr.backend` — trpr backend service
- `trpr.minio` — Minio object storage service

### Run with Docker

```powershell
git clone <repo-url>
cd trpr.backend
docker-compose up -d
```

### Access the app

- Backend service: `http://localhost:5001`
- Minio console: `http://localhost:9001`
- PostgreSQL exposed on `15432`

---

## Running Locally

### Prerequisites

- .NET 10 SDK
- Docker and Docker Compose
- PostgreSQL (only if not using the containerized DB)

### Run the API directly

```powershell
cd c:\Users\yussu\Desktop\cv\trpr.backend
dotnet build .\Web\Api\Api.csproj
dotnet run --project .\Web\Api\Api.csproj
```

---

## Configuration

Configuration is driven by:

- `Web/Api/appsettings.json`
- `Web/Api/appsettings.Development.json`
- module-specific JSON config files loaded in startup
- `.env` when using Docker Compose

### Key settings

- `ConnectionStrings:RommieDb` — PostgreSQL connection string
- `Minio` — object storage endpoint, credentials, and bucket name

---

## Notes

- The backend uses PostgreSQL with PostGIS support via Docker.
- Minio is used for file uploads and public object access.
- Rebus is configured with in-memory transport, allowing the architecture to evolve toward RabbitMQ or another broker later.
- The app is built for maintainability and modular growth rather than a microservices deployment.
