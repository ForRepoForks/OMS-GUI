# Order Management System

A simple order management system API for retailers, built with .NET and PostgreSQL. All services are orchestrated via a single Docker Compose file at the project root. The project follows Test-Driven Development (TDD) and is structured for rapid MVP delivery.

---

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (or LTS version)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/)

## Getting Started

1. **Clone the repository**
2. **Start the API and database using Docker Compose:**
   ```sh
   docker compose up --build
   ```
   - API will be available at `http://localhost:5000`
   - PostgreSQL will be available at `localhost:5432` (user: `omsuser`, password: `omspassword`, db: `omsdb`)

3. **Apply EF Core migrations (if/when implemented):**
   ```sh
   docker compose exec api dotnet ef database update
   ```

## Test-Driven Development (TDD)
- All features begin with writing or updating automated tests.
- To run tests:
  ```sh
  dotnet test
  ```

## Project Structure
- `OrderManagementSystem.API/` - Main API project (controllers, models, services, data)
- `OrderManagementSystem.Tests/` - Automated tests
- `docker-compose.yml` - Orchestrates API and database

## Environment Variables
- Connection strings and secrets are managed in `appsettings.json` and Docker Compose environment variables.

## Additional Notes
- See `FEATURES.md` for progress tracking and development roadmap.
- For API documentation, Swagger will be available at `/swagger` when running locally.