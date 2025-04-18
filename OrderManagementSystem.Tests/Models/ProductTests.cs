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

        [Fact]
        public void Product_CannotBeCreated_WithNegativePrice()
        {
            Assert.Throws<System.ArgumentException>(() => new Product { Name = "Invalid", Price = -1m });
        }

        [Fact]
        public void Product_CanBeCreated_WithLongName()
        {
            var longName = new string('A', 256);
            var product = new Product { Name = longName, Price = 10m };
            Assert.Equal(longName, product.Name);
        }

        [Fact]
        public void Product_CanBeCreated_WithMaxPrice()
        {
            var product = new Product { Name = "Expensive", Price = decimal.MaxValue };
            Assert.Equal(decimal.MaxValue, product.Price);
        }
    }
}
