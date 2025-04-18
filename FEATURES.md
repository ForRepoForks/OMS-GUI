# Order Management System – Feature Tracking

This document tracks the features and work chunks for the Order Management System project. **All development will follow Test-Driven Development (TDD): write tests before implementing features. Core to everything is a single Docker Compose file at the project root, orchestrating all services and dependencies.** Each chunk is sized for a commit and ~30–60 minutes of work, aiming for a total of 8 hours (~8–12 commits).

---

## 1. Project Setup & Architecture
- [x] Initialize .NET solution and git repository
- [x] Set up project structure (Simple monolithic architecture; all logic in API project, organized by folders)
- [ ] Configure database connection (MongoDB or PostgreSQL)
- [ ] Add README with prerequisites and launch steps
- [ ] Create and maintain a single Docker Compose file at the project root to orchestrate all services (API, database, etc.)

## 2. Product Management
- [ ] Write tests for Product entity/model (name, price)
- [ ] Create Product entity/model (name, price)
- [ ] Write tests for Create Product API endpoint
- [ ] Implement Create Product API endpoint
- [ ] Write tests for Get/List Products API endpoint
- [ ] Implement Get/List Products API endpoint
- [ ] Write tests for Product search by name
- [ ] Implement Product search by name
- [ ] Write tests for Apply Discount to Product (percentage & quantity threshold)
- [ ] Implement Apply Discount to Product (percentage & quantity threshold)
- [ ] Write tests for product endpoint input validation
- [ ] Input validation for product endpoints

## 3. Order Management
- [ ] Write tests for Order entity/model (with product list and quantities)
- [ ] Create Order entity/model (with product list and quantities)
- [ ] Write tests for Create Order API endpoint
- [ ] Implement Create Order API endpoint
- [ ] Write tests for Get/List Orders API endpoint
- [ ] Implement Get/List Orders API endpoint
- [ ] Write tests for order endpoint input validation
- [ ] Input validation for order endpoints

## 4. Invoices
- [ ] Write tests for Order Invoice endpoint (show product name, quantity, discount %, amount, total)
- [ ] Implement Order Invoice endpoint (show product name, quantity, discount %, amount, total)

## 5. Reporting
- [ ] Write tests for Discounted Product Report endpoint (product name, discount %, number of orders, total amount)
- [ ] Implement Discounted Product Report endpoint (product name, discount %, number of orders, total amount)

## 6. Documentation & Quality
- [ ] Add Swagger/OpenAPI API documentation
- [ ] Add automated tests (unit/integration)
- [ ] Add input validation for all endpoints

## 7. Deployment & CI
- [ ] Ensure all services (API, database, etc.) are orchestrated through the single Docker Compose file at the project root
- [ ] Add Docker/Docker Compose for containerization
- [ ] Set up Continuous Integration (CI)

---

## Commit Strategy
- Each checkbox represents a logical commit.
- For each feature, write tests first, then implement the feature.
- Mark tasks as complete as you progress.
- Use clear commit messages reflecting the feature or chunk implemented.

---

## Notes
- Prioritize core features first, then bonus/non-functional requirements as time allows.
- Adjust chunk sizes if you need to speed up or slow down.
- Track time spent per chunk if possible for future estimation.
