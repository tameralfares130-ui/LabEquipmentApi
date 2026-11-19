# LabEquipmentApi — Laboratory Equipment Inventory

A controller-based **ASP.NET Core Web API (.NET 10)** with **PostgreSQL**, **EF Core Code First**, **Mapster**, **FluentValidation**, and **Swagger UI**. Includes **required items 1–5** and **optional item 6** from the exercise PDF.

## Prerequisites

- .NET 10 SDK and Visual Studio 2026 (or another .NET-capable IDE)
- PostgreSQL running locally, with a PostgreSQL role that can create tables
- EF CLI tool version 10.x (`dotnet ef --version`); install if necessary:
  ```bash
  dotnet tool install --global dotnet-ef --version 10.0.11
  ```

## Setup

1. Create an empty database called `lab_equipment` in pgAdmin or `psql`:
   ```sql
   CREATE DATABASE lab_equipment;
   ```
2. Set your PostgreSQL connection string. The committed `appsettings.json` has the database name and username but **no password**. For local development, run from the project directory:
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:LabDbConnection" "Host=localhost;Port=5432;Database=lab_equipment;Username=postgres;Password=YOUR_PASSWORD"
   ```
   Alternatively, set the environment variable `ConnectionStrings__LabDbConnection` to the full connection string. **Never commit your password.**
3. Restore dependencies and apply the **included** `InitialCreate` migration:
   ```bash
   dotnet restore
   dotnet ef database update
   ```
4. Run the API in development:
   ```bash
   dotnet run --launch-profile http
   ```
5. Browse to **http://localhost:5100/swagger** (OpenAPI JSON: `http://localhost:5100/openapi/v1.json`). For Visual Studio's HTTPS profile, use the HTTPS URL it displays plus `/swagger`. `GET /` returning 404 is normal.

> The migration source is already included in `Migrations/`; **do not run** `dotnet ef migrations add InitialCreate` again. `dotnet ef database update` will apply it. A new migration, if needed, should have a new name.

## Seed data

| Id | Name | Room | IsAvailable |
|---:|---|---|---|
| 1 | Projector | Lab 10 | true |
| 2 | Laptop | Lab 10 | false |
| 3 | Monitor | Lab 11 | true |

## Endpoints

| Method | Route | Result |
|---|---|---|
| GET | `/api/equipment` | All equipment |
| GET | `/api/equipment/{id}` | One item; 404 if not found |
| GET | `/api/equipment/available` | Available equipment sorted by Name, projected to response DTO |
| POST | `/api/equipment` | Create equipment from `{ "name": ..., "room": ... }`; 201 or validation 400 |
| PUT | `/api/equipment/{id}` | Update an equipment entity; 204, 400 or 404 |
| DELETE | `/api/equipment/{id}` | Delete item; 204 or 404 |
| GET | `/api/equipment/room-summary` | Optional: equipment totals and available totals by room |

## Assignment verification

From Swagger UI or another HTTP client:

1. `GET /api/equipment` → 200 with the three seed records on a new database.
2. `GET /api/equipment/1` → 200 with Projector.
3. `GET /api/equipment/999` → 404.
4. `GET /api/equipment/available` → 200 with **Monitor, then Projector**, not Laptop.
5. `POST /api/equipment` with:
   ```json
   { "name": "Keyboard", "room": "Lab 10" }
   ```
   → 201; `Location` header points to `/api/equipment/{newId}`; newly created equipment has `isAvailable: true`.
6. `POST /api/equipment` with `{ "name": "", "room": "Lab 10" }` → 400 with `Name` validation error.
7. `POST /api/equipment` with `{ "name": "Mouse", "room": "" }` → 400 with `Room` validation error.
8. Repeat the available query → **Keyboard** also appears.
9. On a fresh database, optional `GET /api/equipment/room-summary` → `Lab 10: total=2, available=1` and `Lab 11: total=1, available=1`.

## Git history and GitHub submission

This provided Git repository includes separate commits for project setup, database/migration, CRUD/Swagger, available-items DTO, validated POST, and optional room summary. To publish, **create an empty GitHub repository called `LabEquipmentApi`** (without adding README, license or `.gitignore` on GitHub), then run in the project folder:

```bash
git remote add origin https://github.com/YOUR_USERNAME/LabEquipmentApi.git
git push -u origin main
```

Submit `https://github.com/YOUR_USERNAME/LabEquipmentApi` in the Teams assignment. If using the ZIP file, extract it including the hidden `.git` directory so the commits are preserved.
