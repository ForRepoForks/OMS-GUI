# Port Usage Documentation

This project uses several ports for different services. This documentation explains what each port is used for, where it is configured, and which ports are not in use.

## Ports in Use

### 1. Frontend (React/Vite)
- **Dev Server:**
  - Default: **5173** (Vite's default; not explicitly set, but used unless overridden)

### 2. Backend API (ASP.NET Core)
- **API Base URL:**
  - `.env.example` (outdated): `http://localhost:3000` (not used)
  - `.env`: `http://localhost:8080/api` (used by frontend)
  - `src/api.ts`: uses `process.env.REACT_APP_API_BASE_URL` or `http://localhost:8080`
- **Dockerfile:**
  - `EXPOSE 8080` (main backend API HTTP)
  - `EXPOSE 8081` (alt/HTTPS or secondary, if needed)

### 3. Database (PostgreSQL)
- **Port:** `5432` (standard PostgreSQL port)
  - Set in `OrderManagementSystem.API/appsettings.json`

## Ports Mentioned but Not Used
- **3000:** Only in `.env.example` as an old placeholder. Not used in code or config.
- **5000/5001:** ASP.NET Core defaults, but not used (overridden by Dockerfile and configs).

## Summary Table

| Port   | Used For                | Where Set/Used            | Status         |
|--------|-------------------------|---------------------------|---------------|
| 8080   | Backend API (HTTP)      | .env, api.ts, Dockerfile  | **Used**      |
| 8081   | Backend API (alt/HTTPS) | Dockerfile                | Possibly used |
| 5432   | PostgreSQL DB           | appsettings.json          | **Used**      |
| 3000   | (Old API URL)           | .env.example              | **Not used**  |
| 5173   | Vite dev server (front) | (default)                 | Used if not overridden |
| 5000/5001 | ASP.NET Core defaults | (not set)                 | **Not used**  |

## Recommendations
- Clean up `.env.example` to match `.env` (`8080` instead of `3000`).
- Consider documenting ports in the main README.
- Set Vite dev port explicitly in `vite.config.ts` if you want to avoid ambiguity.
