# LauncherHero Starter

> A minimal, production-ready **.NET 8 Web API** template — zero complexity, zero boilerplate decisions. Clone, configure, ship.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

---

## What's Included

This template gives you a working API with auth and CRUD in minutes. Everything lives in a **single project** — no layers, no abstractions, no ceremony.

| Feature | Details |
|---|---|
| **JWT Authentication** | Register & login, access token (2h), HMAC-SHA256 |
| **Password Hashing** | ASP.NET Identity `PasswordHasher` (bcrypt-style) |
| **CRUD Example** | Full create / read / update / delete on the `User` entity |
| **Repository Pattern** | Thin `UserRepository` wrapping EF Core — easy to extend |
| **EF Core + PostgreSQL** | Code-first migrations, auto-applied on startup |
| **Swagger UI** | Interactive API docs out of the box (Development only) |
| **Docker** | Multi-stage Alpine build, non-root user, `docker-compose` included |

---

## Tech Stack

- **Runtime** — .NET 8 / ASP.NET Core 8
- **Database** — PostgreSQL 16 via `Npgsql.EntityFrameworkCore.PostgreSQL`
- **Auth** — `Microsoft.AspNetCore.Authentication.JwtBearer`
- **Docs** — Swashbuckle / Swagger UI
- **Container** — Docker (Alpine runtime, non-root)

---

## Quick Start

### Option A — Docker Compose (recommended)

```bash
git clone https://github.com/mturan07/launcherhero-starter.git
cd launcherhero-starter

docker-compose up --build
```

The API is ready at **`http://localhost:5010`**. PostgreSQL starts first; the API waits until it is healthy, then auto-applies migrations.

Open Swagger UI: **`http://localhost:5010/swagger`**

---

### Option B — Local (.NET CLI)

**Prerequisites:** .NET 8 SDK, PostgreSQL running locally.

```bash
# 1. Clone
git clone https://github.com/mturan07/launcherhero-starter.git
cd launcherhero-starter

# 2. Configure the connection string
#    Edit appsettings.json → ConnectionStrings:DefaultConnection

# 3. Apply migrations
dotnet ef database update

# 4. Run
dotnet run
```

Open Swagger UI: **`http://localhost:5209/swagger`** *(port may differ — check terminal output)*

---

## Configuration

All settings live in `appsettings.json`. The only values you **must** change before going to production:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=starter;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Issuer": "LauncherHero.Starter",
    "Audience": "LauncherHero.Starter",
    "Secret": "CHANGE_THIS_TO_A_STRONG_SECRET_KEY"
  }
}
```

| Key | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | PostgreSQL connection string |
| `Jwt:Secret` | Signing key — **min 32 chars**, keep it secret |
| `Jwt:Issuer` / `Jwt:Audience` | JWT issuer and audience values |

---

## API Reference

### Auth

#### `POST /api/auth/register`
Create a new account.

```json
// Request
{
  "name": "Jane Doe",
  "email": "jane@example.com",
  "password": "MySecret123!"
}

// Response 201
{
  "accessToken": "eyJhbGci...",
  "expiresAt": "2025-01-01T12:00:00Z",
  "user": {
    "id": 1,
    "name": "Jane Doe",
    "email": "jane@example.com",
    "createdAt": "2025-01-01T10:00:00Z"
  }
}
```

Returns `409 Conflict` if the email is already registered.

---

#### `POST /api/auth/login`
Sign in with existing credentials.

```json
// Request
{
  "email": "jane@example.com",
  "password": "MySecret123!"
}

// Response 200 — same shape as /register
```

Returns `401 Unauthorized` on invalid credentials.

---

### Users

> These endpoints demonstrate CRUD patterns. Add `[Authorize]` to restrict them as needed.

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/user` | Get all users |
| `GET` | `/api/user/{id}` | Get user by ID |
| `POST` | `/api/user` | Create user |
| `PUT` | `/api/user/{id}` | Update user |
| `DELETE` | `/api/user/{id}` | Delete user |

---

## Project Structure

```
LauncherHero.Starter/
├── Controllers/
│   ├── AuthController.cs       # POST /register  POST /login
│   └── UserController.cs       # Full CRUD example
├── Data/
│   ├── AppDbContext.cs
│   └── Repositories/
│       └── UserRepository.cs
├── Migrations/                 # EF Core migrations (auto-applied on startup)
├── Models/
│   ├── User.cs
│   ├── AuthResponse.cs
│   ├── LoginRequest.cs
│   ├── RegisterRequest.cs
│   ├── UserCreateRequest.cs
│   ├── UserUpdateRequest.cs
│   └── UserDto.cs
├── Services/
│   ├── AuthService.cs          # JWT generation, password hashing
│   └── UserService.cs
├── appsettings.json
├── docker-compose.yml
└── Dockerfile
```

---

## Extending This Template

This starter is intentionally small. Common next steps:

| What | How |
|---|---|
| **Protect endpoints** | Add `[Authorize]` to controllers or individual actions |
| **Add refresh tokens** | Persist a `RefreshToken` entity; issue alongside access token |
| **Add roles** | Extend `User` with a `Role` field; add `ClaimTypes.Role` to JWT claims |
| **Add more entities** | Create `Model` → `Repository` → `Service` → `Controller` following the existing pattern |
| **Switch to Clean Architecture** | See [LauncherHero Scale](https://github.com/mturan07/launcherhero-scale) |

---

## LauncherHero Template Family

| Template | Architecture | Best for |
|---|---|---|
| **Starter** *(this)* | Single project | Learning, prototypes, small APIs |
| **Scale** | Clean Architecture + CQRS | Production monoliths |
| **Pro** | Modular + Enterprise patterns | Large teams, complex domains |

---

## License

MIT © [Murat Turan](https://github.com/mturan07)
