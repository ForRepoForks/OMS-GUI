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
            var orders = await response.Content.ReadFromJsonAsync<List<OrderResponse>>();
            Assert.NotNull(orders);
            Assert.Empty(orders);
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
            var orders = await response.Content.ReadFromJsonAsync<List<OrderResponse>>();
            Assert.NotNull(orders);
            Assert.True(orders.Count == 2); // Should only be the two we created
            Assert.Contains(orders, o => o.Id == created1.Id && o.Items.Count == 1 && o.Items[0].ProductId == prod1.Id && o.Items[0].Quantity == 2);
            Assert.Contains(orders, o => o.Id == created2.Id && o.Items.Count == 1 && o.Items[0].ProductId == prod2.Id && o.Items[0].Quantity == 3);
        }
    }
}
