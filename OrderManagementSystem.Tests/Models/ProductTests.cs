using Xunit;
using OrderManagementSystem.API.Models;

namespace OrderManagementSystem.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Product_CanBeCreated_WithNameAndPrice()
        {
            var product = new Product { Name = "Test Product", Price = 9.99m };
            Assert.Equal("Test Product", product.Name);
            Assert.Equal(9.99m, product.Price);
        }
    }
}
