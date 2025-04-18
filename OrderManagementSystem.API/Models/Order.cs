using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OrderManagementSystem.API.Models
{
    public class Order
    {
        public class OrderItem
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }

            public OrderItem(Product product, int quantity)
            {
                Product = product ?? throw new ArgumentNullException(nameof(product));
                Quantity = quantity;
            }
        }

        public IReadOnlyList<OrderItem> Items { get; }

        public Order(IEnumerable<(Product product, int quantity)> items)
        {
            if (items == null || !items.Any())
                throw new ArgumentException("Order must have at least one product.", nameof(items));

            Items = items.Select(i => new OrderItem(i.product, i.quantity)).ToList().AsReadOnly();
        }
    }
}
