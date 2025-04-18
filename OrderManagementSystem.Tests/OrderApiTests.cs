using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementSystem.API.Models;
using Xunit;

namespace OrderManagementSystem.Tests
{
    public class OrderApiTests : IClassFixture<WebApplicationFactory<OrderManagementSystem.API.Program>>
    {
        private readonly WebApplicationFactory<OrderManagementSystem.API.Program> _factory;

        public OrderApiTests(WebApplicationFactory<OrderManagementSystem.API.Program> factory)
        {
            _factory = factory;
        }

        private async Task CleanupDatabaseAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderManagementSystem.API.Data.OrderManagementContext>();
            db.OrderItems.RemoveRange(db.OrderItems);
            db.Orders.RemoveRange(db.Orders);
            db.Products.RemoveRange(db.Products);
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateOrder_ValidInput_ReturnsCreatedOrder()
        {
            await CleanupDatabaseAsync();
            // Arrange: create products first
            var client = _factory.CreateClient();
            var p1 = new Product { Name = "OrderTest Product 1", Price = 5.00m };
            var p2 = new Product { Name = "OrderTest Product 2", Price = 7.50m };
            var resp1 = await client.PostAsJsonAsync("/api/products", p1);
            var resp2 = await client.PostAsJsonAsync("/api/products", p2);
            Assert.Equal(HttpStatusCode.Created, resp1.StatusCode);
            Assert.Equal(HttpStatusCode.Created, resp2.StatusCode);
            var prod1 = await resp1.Content.ReadFromJsonAsync<Product>();
            var prod2 = await resp2.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(prod1);
            Assert.NotNull(prod2);

            var orderRequest = new
            {
                items = new[]
                {
                    new { productId = prod1.Id, quantity = 2 },
                    new { productId = prod2.Id, quantity = 3 }
                }
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/orders", orderRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var created = await response.Content.ReadFromJsonAsync<OrderResponse>();
            Assert.NotNull(created);
            Assert.Equal(2, created.Items.Count);
            Assert.Contains(created.Items, i => i.ProductId == prod1.Id && i.Quantity == 2);
            Assert.Contains(created.Items, i => i.ProductId == prod2.Id && i.Quantity == 3);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CreateOrder_InvalidInput_ReturnsBadRequest(object items)
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            var orderRequest = new { items };
            var response = await client.PostAsJsonAsync("/api/orders", orderRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_EmptyItemsList_ReturnsBadRequest()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            var orderRequest = new { items = new object[] { } };
            var response = await client.PostAsJsonAsync("/api/orders", orderRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_MissingProductId_ReturnsBadRequest()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            var orderRequest = new { items = new[] { new { quantity = 2 } } };
            var response = await client.PostAsJsonAsync("/api/orders", orderRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_MissingQuantity_ReturnsBadRequest()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            // First, create a valid product
            var productResp = await client.PostAsJsonAsync("/api/products", new Product { Name = "Test Product", Price = 1.00m });
            Assert.Equal(HttpStatusCode.Created, productResp.StatusCode);
            var product = await productResp.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(product);
            var orderRequest = new { items = new[] { new { productId = product.Id } } };
            var response = await client.PostAsJsonAsync("/api/orders", orderRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CreateOrder_QuantityLessThanOne_ReturnsBadRequest(int quantity)
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            // First, create a valid product
            var productResp = await client.PostAsJsonAsync("/api/products", new Product { Name = "Test Product", Price = 1.00m });
            Assert.Equal(HttpStatusCode.Created, productResp.StatusCode);
            var product = await productResp.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(product);
            var orderRequest = new { items = new[] { new { productId = product.Id, quantity } } };
            var response = await client.PostAsJsonAsync("/api/orders", orderRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_MissingAllFieldsInItem_ReturnsBadRequest()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            var orderRequest = new { items = new[] { new { } } };
            var response = await client.PostAsJsonAsync("/api/orders", orderRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_NonExistentProduct_ReturnsNotFound()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            var orderRequest = new
            {
                items = new[] { new { productId = 99999, quantity = 1 } }
            };
            var response = await client.PostAsJsonAsync("/api/orders", orderRequest);
            Assert.True(response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest);
        }

        public class OrderResponse
        {
            public int Id { get; set; }
            public List<OrderItemResponse> Items { get; set; } = new();
        }
        public class OrderItemResponse
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        [Fact]
        public async Task GetOrders_Empty_ReturnsEmptyList()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/orders");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var paged = await response.Content.ReadFromJsonAsync<PagedResult<OrderResponse>>();
            Assert.NotNull(paged);
            Assert.NotNull(paged.Items);
            Assert.Empty(paged.Items);
        }

        [Fact]
        public async Task GetOrderInvoice_ReturnsCorrectInvoice()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();

            // Create products
            var product1 = new Product { Name = "Apple", Price = 10m };
            var product2 = new Product { Name = "Banana", Price = 20m, DiscountPercentage = 25m, DiscountQuantityThreshold = 2 };
            var resp1 = await client.PostAsJsonAsync("/api/products", product1);
            var resp2 = await client.PostAsJsonAsync("/api/products", product2);
            Assert.Equal(HttpStatusCode.Created, resp1.StatusCode);
            Assert.Equal(HttpStatusCode.Created, resp2.StatusCode);
            var p1 = await resp1.Content.ReadFromJsonAsync<Product>();
            var p2 = await resp2.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(p1);
            Assert.NotNull(p2);

            // Place order: 1 Apple (no discount), 3 Banana (discount applies to 2+)
            var orderRequest = new
            {
                items = new[]
                {
                    new { productId = p1.Id, quantity = 1 },
                    new { productId = p2.Id, quantity = 3 }
                }
            };
            var orderResp = await client.PostAsJsonAsync("/api/orders", orderRequest);
            Assert.Equal(HttpStatusCode.Created, orderResp.StatusCode);
            var createdOrder = await orderResp.Content.ReadFromJsonAsync<OrderResponse>();
            Assert.NotNull(createdOrder);

            // Act: get invoice
            var invoiceResp = await client.GetAsync($"/api/orders/{createdOrder.Id}/invoice");
            Assert.Equal(HttpStatusCode.OK, invoiceResp.StatusCode);
            var invoice = await invoiceResp.Content.ReadFromJsonAsync<OrderInvoiceResponse>();
            Assert.NotNull(invoice);
            Assert.Equal(2, invoice.Products.Count);

            var appleLine = invoice.Products.Find(x => x.ProductName == "Apple");
            var bananaLine = invoice.Products.Find(x => x.ProductName == "Banana");
            Assert.NotNull(appleLine);
            Assert.NotNull(bananaLine);

            // Apple: no discount
            Assert.Equal(1, appleLine.Quantity);
            Assert.Equal(0m, appleLine.DiscountPercent);
            Assert.Equal(10m, appleLine.Amount);
            // Banana: 25% discount applies to all 3 (since quantity >= threshold)
            Assert.Equal(3, bananaLine.Quantity);
            Assert.Equal(25m, bananaLine.DiscountPercent);
            Assert.Equal(45m, bananaLine.Amount); // 20 * 3 * 0.75 = 45
            // Total
            Assert.Equal(55m, invoice.TotalAmount);
        }

        [Fact]
        public async Task GetOrderInvoice_NonExistentOrder_ReturnsNotFound()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/orders/99999/invoice");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        public class OrderInvoiceResponse
        {
            public List<OrderInvoiceProduct> Products { get; set; } = new();
            public decimal TotalAmount { get; set; }
        }
        public class OrderInvoiceProduct
        {
            public required string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal DiscountPercent { get; set; }
            public decimal Amount { get; set; }
        }

        [Fact]
        public async Task GetOrders_ReturnsCreatedOrdersWithItems()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            // Create products
            var p1 = new Product { Name = "GetList Product 1", Price = 1.00m };
            var p2 = new Product { Name = "GetList Product 2", Price = 2.00m };
            var resp1 = await client.PostAsJsonAsync("/api/products", p1);
            var resp2 = await client.PostAsJsonAsync("/api/products", p2);
            Assert.Equal(HttpStatusCode.Created, resp1.StatusCode);
            Assert.Equal(HttpStatusCode.Created, resp2.StatusCode);
            var prod1 = await resp1.Content.ReadFromJsonAsync<Product>();
            var prod2 = await resp2.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(prod1);
            Assert.NotNull(prod2);

            // Create order 1
            var orderReq1 = new { items = new[] { new { productId = prod1.Id, quantity = 2 } } };
            var respOrder1 = await client.PostAsJsonAsync("/api/orders", orderReq1);
            Assert.Equal(HttpStatusCode.Created, respOrder1.StatusCode);
            var created1 = await respOrder1.Content.ReadFromJsonAsync<OrderResponse>();
            Assert.NotNull(created1);

            // Create order 2
            var orderReq2 = new { items = new[] { new { productId = prod2.Id, quantity = 3 } } };
            var respOrder2 = await client.PostAsJsonAsync("/api/orders", orderReq2);
            Assert.Equal(HttpStatusCode.Created, respOrder2.StatusCode);
            var created2 = await respOrder2.Content.ReadFromJsonAsync<OrderResponse>();
            Assert.NotNull(created2);

            // Act
            var response = await client.GetAsync("/api/orders");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var paged = await response.Content.ReadFromJsonAsync<PagedResult<OrderResponse>>();
            Assert.NotNull(paged);
            Assert.NotNull(paged.Items);
            Assert.True(paged.Items.Count == 2); // Should only be the two we created
            Assert.Contains(paged.Items, o => o.Id == created1.Id && o.Items.Count == 1 && o.Items[0].ProductId == prod1.Id && o.Items[0].Quantity == 2);
            Assert.Contains(paged.Items, o => o.Id == created2.Id && o.Items.Count == 1 && o.Items[0].ProductId == prod2.Id && o.Items[0].Quantity == 3);
        }

        [Fact]
        public async Task GetDiscountedProductReport_ReturnsCorrectReport()
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();

            // Create products: one with discount, one without, one with discount but never ordered above threshold
            var prodDiscount = new Product { Name = "Discounted", Price = 100m, DiscountPercentage = 20m, DiscountQuantityThreshold = 2 };
            var prodNoDiscount = new Product { Name = "NoDiscount", Price = 50m };
            var prodUnusedDiscount = new Product { Name = "UnusedDiscount", Price = 80m, DiscountPercentage = 10m, DiscountQuantityThreshold = 5 };

            var resp1 = await client.PostAsJsonAsync("/api/products", prodDiscount);
            var resp2 = await client.PostAsJsonAsync("/api/products", prodNoDiscount);
            var resp3 = await client.PostAsJsonAsync("/api/products", prodUnusedDiscount);
            Assert.Equal(HttpStatusCode.Created, resp1.StatusCode);
            Assert.Equal(HttpStatusCode.Created, resp2.StatusCode);
            Assert.Equal(HttpStatusCode.Created, resp3.StatusCode);
            var pDiscount = await resp1.Content.ReadFromJsonAsync<Product>();
            var pNoDiscount = await resp2.Content.ReadFromJsonAsync<Product>();
            var pUnusedDiscount = await resp3.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(pDiscount);
            Assert.NotNull(pNoDiscount);
            Assert.NotNull(pUnusedDiscount);

            // Place orders:
            // 1. Discounted product, quantity 2 (discount applies)
            var order1 = new { items = new[] { new { productId = pDiscount.Id, quantity = 2 } } };
            var respOrder1 = await client.PostAsJsonAsync("/api/orders", order1);
            Assert.Equal(HttpStatusCode.Created, respOrder1.StatusCode);
            // 2. Discounted product, quantity 1 (discount does not apply)
            var order2 = new { items = new[] { new { productId = pDiscount.Id, quantity = 1 } } };
            var respOrder2 = await client.PostAsJsonAsync("/api/orders", order2);
            Assert.Equal(HttpStatusCode.Created, respOrder2.StatusCode);
            // 3. Discounted product, quantity 3 (discount applies)
            var order3 = new { items = new[] { new { productId = pDiscount.Id, quantity = 3 } } };
            var respOrder3 = await client.PostAsJsonAsync("/api/orders", order3);
            Assert.Equal(HttpStatusCode.Created, respOrder3.StatusCode);
            // 4. NoDiscount product, quantity 4
            var order4 = new { items = new[] { new { productId = pNoDiscount.Id, quantity = 4 } } };
            var respOrder4 = await client.PostAsJsonAsync("/api/orders", order4);
            Assert.Equal(HttpStatusCode.Created, respOrder4.StatusCode);
            // 5. UnusedDiscount product, quantity 2 (below threshold)
            var order5 = new { items = new[] { new { productId = pUnusedDiscount.Id, quantity = 2 } } };
            var respOrder5 = await client.PostAsJsonAsync("/api/orders", order5);
            Assert.Equal(HttpStatusCode.Created, respOrder5.StatusCode);

            // Act: Call the report endpoint
            var response = await client.GetAsync("/api/reports/discounted-products");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var report = await response.Content.ReadFromJsonAsync<List<DiscountedProductReportItem>>();
            Assert.NotNull(report);
            // Only the Discounted product should appear, since UnusedDiscount never met threshold
            Assert.Single(report);
            var discounted = report[0];
            Assert.Equal(pDiscount.Name, discounted.ProductName);
            Assert.Equal(pDiscount.DiscountPercentage, discounted.DiscountPercent);
            // Orders where discount applied: order1 (2), order3 (3)
            Assert.Equal(2, discounted.NumberOfOrders);
            // Total amount: (100*2*0.8) + (100*3*0.8) = 160 + 240 = 400
            Assert.Equal(400m, discounted.TotalAmount);
        }

        public class DiscountedProductReportItem
        {
            public required string ProductName { get; set; }
            public decimal DiscountPercent { get; set; }
            public int NumberOfOrders { get; set; }
            public decimal TotalAmount { get; set; }
        }
    }
}
