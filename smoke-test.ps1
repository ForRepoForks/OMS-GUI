<#
    Smoke Test for Order Management System API
    ------------------------------------------------------
    This script simulates real user flows to verify the core functionality of the Order Management System API.

    What it does:
    - Lists products
    - Creates multiple products (with and without discounts)
    - Searches for products by partial name
    - Applies a discount to one product
    - Attempts to create an invalid product (expecting failure)
    - Creates multiple orders with different product combinations
    - Retrieves invoices for each order (including discounted scenarios)
    - Attempts to fetch an invoice for a non-existent order (expecting failure)
    - Checks the discounted product report

    Usage:
    - Requires PowerShell 7+
    - Ensure the API is running and accessible at http://localhost:8080
    - Run: powershell -ExecutionPolicy Bypass -File .\smoke-test.ps1

    This script is intended for local development, CI smoke testing, and regression checks.
#>
# Smoke test for Order Management System API
# Requires PowerShell 7+

$baseUrl = "http://localhost:8080/api"

function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Url,
        [object]$Body = $null
    )
    Write-Host "Testing $Method $Url" -ForegroundColor Cyan
    try {
        if ($Body) {
            $response = Invoke-RestMethod -Method $Method -Uri $Url -Body ($Body | ConvertTo-Json) -ContentType 'application/json' -ErrorAction Stop
        } else {
            $response = Invoke-RestMethod -Method $Method -Uri $Url -ErrorAction Stop
        }
        Write-Host "Success: $($response | ConvertTo-Json -Compress)" -ForegroundColor Green
        return $response
    } catch {
        Write-Host "Failed: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# 1. List products
Test-Endpoint GET "$baseUrl/products"

# 2. Create multiple products
$productsToCreate = @(
    @{ name = "SmokeTestProductA"; price = 9.99 },
    @{ name = "SmokeTestProductB"; price = 19.99 },
    @{ name = "DiscountedProduct"; price = 29.99 }
)
$createdProducts = @()
foreach ($prod in $productsToCreate) {
    $result = Test-Endpoint POST "$baseUrl/products" $prod
    if ($result -and $result.id) { $createdProducts += $result }
}

# 3. Search for products by partial name
Test-Endpoint GET "$baseUrl/products?name=SmokeTest"

# 4. Apply discount to one product
$discounted = $createdProducts | Where-Object { $_.name -eq "DiscountedProduct" }
if ($discounted) {
    $discount = @{ percentage = 20; quantityThreshold = 2 }
    Test-Endpoint PUT "$baseUrl/products/$($discounted.id)/discount" $discount
}

# 5. Try to create a product with invalid data (should fail)
$invalidProduct = @{ name = ""; price = -5 }
Test-Endpoint POST "$baseUrl/products" $invalidProduct

# 6. Create multiple orders
$orders = @()
if ($createdProducts.Count -ge 2) {
    # Order 1: 1x each product
    $order1 = @{ items = @(
        @{ productId = $createdProducts[0].id; quantity = 1 },
        @{ productId = $createdProducts[1].id; quantity = 1 }
    ) }
    $orders += Test-Endpoint POST "$baseUrl/orders" $order1
    # Order 2: 3x discounted product (should trigger discount)
    if ($discounted) {
        $order2 = @{ items = @(@{ productId = $discounted.id; quantity = 3 }) }
        $orders += Test-Endpoint POST "$baseUrl/orders" $order2
    }
}

# 7. List orders
Test-Endpoint GET "$baseUrl/orders"

# 8. Check invoices for each order
foreach ($order in $orders) {
    if ($order -and $order.id) {
        Test-Endpoint GET "$baseUrl/orders/$($order.id)/invoice"
    }
}

# 9. Try to get invoice for non-existent order (should fail)
Test-Endpoint GET "$baseUrl/orders/999999/invoice"

# 10. Get discounted product report
Test-Endpoint GET "http://localhost:8080/api/reports/discounted-products"

# 3. List products with search
Test-Endpoint GET "$baseUrl/products?name=SmokeTestProduct"

# 4. Apply discount to product
if ($newProduct -and $newProduct.id) {
    $discount = @{ percentage = 10; quantityThreshold = 3 }
    Test-Endpoint PUT "$baseUrl/products/$($newProduct.id)/discount" $discount
}

# 5. Create order
if ($newProduct -and $newProduct.id) {
    $order = @{ items = @(@{ productId = $newProduct.id; quantity = 5 }) }
    $newOrder = Test-Endpoint POST "$baseUrl/orders" $order
}

# 6. List orders
Test-Endpoint GET "$baseUrl/orders"

# 7. Get order invoice
if ($newOrder -and $newOrder.id) {
    Test-Endpoint GET "$baseUrl/orders/$($newOrder.id)/invoice"
}

# 8. Get discounted product report
Test-Endpoint GET "http://localhost:8080/api/reports/discounted-products"
