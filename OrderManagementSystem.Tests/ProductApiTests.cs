using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementSystem.API.Models;
using Xunit;

namespace OrderManagementSystem.Tests
{
    public class ProductApiTests : IClassFixture<WebApplicationFactory<OrderManagementSystem.API.Program>>
    {
        private async Task CleanupDatabaseAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderManagementSystem.API.Data.OrderManagementContext>();
            db.OrderItems.RemoveRange(db.OrderItems);
            db.Orders.RemoveRange(db.Orders);
            db.Products.RemoveRange(db.Products);
            await db.SaveChangesAsync();
        }

        [Theory]
        [InlineData(null, 10)]
        [InlineData("", 10)]
        [InlineData("Valid Name", 0)]
        [InlineData("Valid Name", -5)]
        public async Task CreateProduct_InvalidInput_ReturnsBadRequest(string name, decimal price)
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            var product = new Product { Name = name, Price = price };
            var response = await client.PostAsJsonAsync("/api/products", product);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(101, 10)]
        [InlineData(10, 0)]
        [InlineData(10, -5)]
        public async Task ApplyDiscount_InvalidInput_ReturnsBadRequest(decimal percentage, int quantityThreshold)
        {
            await CleanupDatabaseAsync();
            var client = _factory.CreateClient();
            var newProduct = new Product { Name = "Valid Product", Price = 100m };
            var createResponse = await client.PostAsJsonAsync("/api/products", newProduct);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(created);

            var discount = new { Percentage = percentage, QuantityThreshold = quantityThreshold };
            var discountResponse = await client.PutAsJsonAsync($"/api/products/{created.Id}/discount", discount);
            Assert.Equal(HttpStatusCode.BadRequest, discountResponse.StatusCode);
        }

        private readonly WebApplicationFactory<OrderManagementSystem.API.Program> _factory;

        public ProductApiTests(WebApplicationFactory<OrderManagementSystem.API.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedProduct()
        {
            await CleanupDatabaseAsync();
            // Arrange
            var client = _factory.CreateClient();
            var newProduct = new Product { Name = "Integration Test Product", Price = 12.34m };

            // Act
            var response = await client.PostAsJsonAsync("/api/products", newProduct);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var created = await response.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(created);
            Assert.Equal(newProduct.Name, created.Name);
            Assert.Equal(newProduct.Price, created.Price);
            Assert.True(created.Id > 0);
        }
        [Fact]
        public async Task GetProducts_ReturnsListIncludingCreatedProduct()
        {
            await CleanupDatabaseAsync();
            // Arrange
            var client = _factory.CreateClient();
            var newProduct = new Product { Name = "List Test Product", Price = 55.55m };
            var createResponse = await client.PostAsJsonAsync("/api/products", newProduct);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<Product>();

            // Act
            var response = await client.GetAsync("/api/products");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadFromJsonAsync<Product[]>();
            Assert.NotNull(products);
            Assert.Contains(products, p => p.Id == created.Id && p.Name == newProduct.Name && p.Price == newProduct.Price);
        }

        [Fact]
        public async Task GetProducts_SearchByName_ReturnsMatchingProducts()
        {
            await CleanupDatabaseAsync();
            // Arrange
            var client = _factory.CreateClient();
            var productsToCreate = new[]
            {
                new Product { Name = "Apple", Price = 1.00m },
                new Product { Name = "Banana", Price = 2.00m },
                new Product { Name = "Green Apple", Price = 1.50m },
                new Product { Name = "Pineapple", Price = 3.00m }
            };
            foreach (var p in productsToCreate)
            {
                var resp = await client.PostAsJsonAsync("/api/products", p);
                Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
                var created = await resp.Content.ReadFromJsonAsync<Product>();
                Assert.NotNull(created);
            }

            // Act
            var response = await client.GetAsync("/api/products?name=Apple");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadFromJsonAsync<Product[]>();
            Assert.NotNull(products);
            Assert.All(products, p => Assert.True(p.Name.IndexOf("Apple", System.StringComparison.OrdinalIgnoreCase) >= 0));
            Assert.Contains(products, p => p.Name == "Apple");
            Assert.Contains(products, p => p.Name == "Green Apple");
            Assert.Contains(products, p => p.Name == "Pineapple");
            Assert.DoesNotContain(products, p => p.Name == "Banana");
        }

        [Fact]
        public async Task ApplyDiscountToProduct_StoresDiscountCorrectly()
        {
            await CleanupDatabaseAsync();
            // Arrange
            var client = _factory.CreateClient();
            var newProduct = new Product { Name = "Discounted Product", Price = 100m };
            var createResponse = await client.PostAsJsonAsync("/api/products", newProduct);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(created);

            var discount = new { Percentage = 15, QuantityThreshold = 10 };

            // Act
            var discountResponse = await client.PutAsJsonAsync($"/api/products/{created.Id}/discount", discount);
            discountResponse.EnsureSuccessStatusCode();
            var updated = await discountResponse.Content.ReadFromJsonAsync<Product>();

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(created.Id, updated.Id);
            Assert.Equal(discount.Percentage, updated.DiscountPercentage);
            Assert.Equal(discount.QuantityThreshold, updated.DiscountQuantityThreshold);
        }
    }
}
