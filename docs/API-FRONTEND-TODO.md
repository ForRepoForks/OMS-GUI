# API-Frontend Integration TODO

This document tracks the work required to ensure the frontend can fully test and exercise every feature of the Order Management System API. The API is fixed and will not change; all tasks are about creating or extending frontend UI or logic to test the existing API endpoints.

## TODO List

### 1. Orders

#### 1.1 Create or extend frontend UI to allow creating orders and test `POST /api/orders`
- [ ] Product Selection UI: Fetch product list from `/api/products` and update the order dialog to allow selecting a product by name (submit its ID).
- [ ] Order Dialog Validation: Ensure the form only allows valid combinations (e.g., positive quantity, product required).
- [ ] API Integration: Construct the payload in the required format (`{ items: [{ productId, quantity }] }`) and send a POST request to `/api/orders` on save.
- [ ] Error Handling & Feedback: Display errors from the API to the user and show loading/progress indicators as needed.
- [ ] Order List Refresh: On successful creation, refresh the orders list to show the new order.
- [ ] Code Quality & Reuse: Refactor any duplicated code (e.g., product fetching) into reusable hooks or utilities.

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
