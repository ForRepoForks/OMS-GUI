# Order Management System

A simple order management system API for retailers, built with .NET 8 and PostgreSQL. All services are orchestrated via a single Docker Compose file at the project root. The project follows Test-Driven Development (TDD) and a clear commit strategy.

---

## Running on a New Development Environment

To get started on a new development machine:

1. **Install prerequisites:**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download)
   - [Docker Desktop](https://www.docker.com/products/docker-desktop)
   - [Docker Compose](https://docs.docker.com/compose/)

2. **Clone the repository:**
   ```sh
   git clone <your-repo-url>
   cd OrderManagementSystem
   ```

3. **Start the API and database:**
   ```sh
   docker compose up --build
   ```
   - The API will be available at http://localhost:5000
   - PostgreSQL will be available at localhost:5432 (user: `omsuser`, password: `omspassword`, db: `omsdb`)

4. **Apply EF Core migrations (if not already applied):**
   ```sh
   docker compose exec api dotnet ef database update
   ```

5. **Run automated tests:**
   ```sh
   dotnet test
   ```

6. **Access API documentation:**
   - Open [http://localhost:5000/swagger](http://localhost:5000/swagger) in your browser for Swagger UI and endpoint documentation.

---

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/)

## Getting Started

1. **Clone the repository**
2. **Start the API and database using Docker Compose:**
   ```sh
   docker compose up --build
   ```
   - API: http://localhost:5000
   - PostgreSQL: localhost:5432 (user: `omsuser`, password: `omspassword`, db: `omsdb`)

3. **Apply EF Core migrations (if not already applied):**
   ```sh
   docker compose exec api dotnet ef database update
   ```

4. **Run tests:**
   ```sh
   dotnet test
   ```

## API Endpoints Overview

- **Products**
  - `POST /api/products` — Create a product
  - `GET /api/products` — List products (supports search & pagination)
  - `PUT /api/products/{id}/discount` — Apply discount to product
- **Orders**
  - `POST /api/orders` — Create an order
  - `GET /api/orders` — List orders (supports pagination)
  - `GET /api/orders/{id}/invoice` — Get order invoice
- **Reports**
  - `GET /api/reports/discounted-products` — Discounted product report

See Swagger UI for full details and request/response schemas.

## Pagination
All list endpoints (`GET /api/products`, `GET /api/orders`) support pagination:
- Query parameters: `page` (default: 1), `pageSize` (default: 10, max: 100)
- Response is wrapped in a `PagedResult<T>`:
  ```json
  {
    "items": [ ... ],
    "totalCount": 123,
    "page": 1,
    "pageSize": 10
  }
  ```

## Test-Driven Development (TDD) & Commit Strategy
- All features begin with writing or updating automated tests.
- Use conventional commit messages (e.g., `feat`, `fix`, `test`, `docs`).
- See `OrderManagementSystem.Tests/` for test coverage.

## Project Structure
- `OrderManagementSystem.API/` — Main API project (controllers, models, services, data)
- `OrderManagementSystem.Tests/` — Automated tests
- `docker-compose.yml` — Orchestrates API and database
- `FEATURES.md` — Progress tracking and development roadmap

## Environment Variables & Configuration
- Connection strings are managed in `appsettings.json` and Docker Compose environment variables.
- API uses: `Host=db;Port=5432;Database=omsdb;Username=omsuser;Password=omspassword`

## API Documentation
- Swagger UI is available at [http://localhost:5000/swagger](http://localhost:5000/swagger) when running locally.

## Troubleshooting & Warnings
- Some warnings about nullable reference types or EF Core version conflicts may appear during build/test. These do not affect functionality but can be addressed for code quality.
- If you change data models, always update and apply EF Core migrations:
  ```sh
  dotnet ef migrations add <MigrationName>
  docker compose exec api dotnet ef database update
  ```

## Additional Notes
- The project is designed for rapid MVP delivery and can be extended with additional features, validation, and performance optimizations as needed.
- **Design Choice:** The code structure is a simple monolith by design to enable fast iteration and delivery of a minimum viable product (MVP). For larger-scale or long-term projects, introducing layered or modular architecture (e.g., NTier, Onion) is recommended to improve maintainability and scalability.
- See `FEATURES.md` for progress tracking and development roadmap.