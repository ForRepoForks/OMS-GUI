using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderManagementSystem.API.Models;
using Xunit;

namespace OrderManagementSystem.Tests
{
    public class ProductApiTests : IClassFixture<WebApplicationFactory<OrderManagementSystem.API.Program>>
    {
        private readonly WebApplicationFactory<OrderManagementSystem.API.Program> _factory;

        public ProductApiTests(WebApplicationFactory<OrderManagementSystem.API.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedProduct()
        {
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
                resp.EnsureSuccessStatusCode();
            }

            // Act
            var response = await client.GetAsync("/api/products?name=Apple");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadFromJsonAsync<Product[]>();
            Assert.NotNull(products);
            Assert.All(products, p => Assert.Contains("Apple", p.Name));
            Assert.Contains(products, p => p.Name == "Apple");
            Assert.Contains(products, p => p.Name == "Green Apple");
            Assert.Contains(products, p => p.Name == "Pineapple");
            Assert.DoesNotContain(products, p => p.Name == "Banana");
        }
    }
}
