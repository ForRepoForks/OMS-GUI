# Order Management System Front-End

This is the React + TypeScript front-end for the Order Management System (OMS). It provides a minimal, realistic UI for managing orders (and optionally customers/products) and is designed to work with the OMS API.

## Prerequisites

- [Node.js (18+ recommended)](https://nodejs.org/)
- [npm](https://www.npmjs.com/)

## Getting Started

1. **Install dependencies:**
   ```sh
   npm install
   ```
2. **Start the development server:**

   ```sh
   npm run dev
   ```

   - The app will be available at [http://localhost:5173](http://localhost:5173)

3. **Configure API URL:**
   - By default, the app expects the OMS API to be running at `http://localhost:8080`.
   - To change the API base URL, set the `REACT_APP_API_BASE_URL` environment variable in a `.env` file at the project root:
     ```env
     REACT_APP_API_BASE_URL=http://localhost:8080
     ```

## Project Structure

- `src/` — React source code
- `public/` — Static assets
- `index.html` — Main HTML template

## Available Scripts

- `npm run dev` — Start development server
- `npm run build` — Build for production
- `npm run preview` — Preview production build
- `npm test` — Run tests
- `npx eslint . --ext .js,.jsx,.ts,.tsx` — Check code for lint errors
- `npx eslint . --ext .js,.jsx,.ts,.tsx --fix` — Auto-fix lint errors
- `npx prettier --write .` — Format code with Prettier

## Linting & Formatting

- ESLint and Prettier are set up for code quality and consistency.
- You can lint and auto-fix code with:
  ```sh
  npx eslint . --ext .js,.jsx,.ts,.tsx --fix
  ```
- You can format all code with:
  ```sh
  npx prettier --write .
  ```

## Running Tests

- Run all tests with:
  ```sh
  npm test
  ```

---

For API documentation and backend setup, see the main project [README.md](../README.md).
