# API-Frontend Integration TODO

This document tracks the work required to ensure the frontend can fully test and exercise every feature of the Order Management System API. The API is fixed and will not change; all tasks are about creating or extending frontend UI or logic to test the existing API endpoints.

## TODO List

### 1. Orders
- [ ] Create or extend frontend UI to allow creating orders and test `POST /api/orders`.
- [ ] Add frontend UI to fetch and display order invoice using `GET /api/orders/{id}/invoice`.

### 2. Products
- [ ] Create or extend frontend UI to allow creating products and test `POST /api/products`.
- [ ] Add frontend UI to set product discounts and test `PUT /api/products/{id}/discount`.

### 3. Reports
- [ ] Add frontend UI to fetch and display the discounted products report (`GET /api/reports/discounted-products`).

### 4. Database Seeding
- [ ] Add frontend UI to trigger and test database seeding (`POST /api/seed`).

## Notes
- All tasks are about missing frontend pieces for testing the API.
- Ensure all new features have basic error handling and user feedback.
- Update this file as progress is made.
