using System.Collections.Generic;
using Xunit;
using OrderManagementSystem.API.Models;

namespace OrderManagementSystem.Tests.Models
{
    public class OrderTests
    {
        [Fact]
        public void Order_CanBeCreated_WithProductListAndQuantities()
        {
            // Arrange
            var product1 = new Product { Name = "Product 1", Price = 10m };
            var product2 = new Product { Name = "Product 2", Price = 20m };
            var items = new List<(Product product, int quantity)>
            {
                (product1, 2),
                (product2, 5)
            };

            // Act
            var order = new Order(items);

            // Assert
            Assert.Equal(2, order.Items.Count);
            Assert.Contains(order.Items, i => i.Product == product1 && i.Quantity == 2);
            Assert.Contains(order.Items, i => i.Product == product2 && i.Quantity == 5);
        }

        [Fact]
        public void Order_ThrowsException_IfProductListIsNullOrEmpty()
        {
            // Arrange, Act & Assert
            Assert.Throws<System.ArgumentException>(() => new Order(null));
            Assert.Throws<System.ArgumentException>(() => new Order(new List<(Product, int)>()));
        }
    }
}
