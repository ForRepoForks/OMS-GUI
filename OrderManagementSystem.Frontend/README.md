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
   - By default, the app expects the OMS API to be running at `http://localhost:5000`.
   - To change the API base URL, set the `VITE_API_BASE_URL` environment variable in a `.env` file at the project root:
     ```env
     VITE_API_BASE_URL=http://localhost:5000
     ```

## Project Structure
- `src/` — React source code
- `public/` — Static assets
- `index.html` — Main HTML template

## Available Scripts
- `npm run dev` — Start development server
- `npm run build` — Build for production
- `npm run preview` — Preview production build

---

For API documentation and backend setup, see the main project [README.md](../README.md).

  plugins: {
    // Add the react-x and react-dom plugins
    'react-x': reactX,
    'react-dom': reactDom,
  },
  rules: {
    // other rules...
    // Enable its recommended typescript rules
    ...reactX.configs['recommended-typescript'].rules,
    ...reactDom.configs.recommended.rules,
  },
})
```
