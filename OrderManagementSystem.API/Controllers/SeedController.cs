using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.API.Data;
using OrderManagementSystem.API.Models;
using System.Threading.Tasks;
using System.Linq;

namespace OrderManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly OrderManagementContext _context;
        public SeedController(OrderManagementContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SeedDatabase()
        {
            // Remove existing data
            _context.OrderItems.RemoveRange(_context.OrderItems);
            _context.Orders.RemoveRange(_context.Orders);
            _context.Products.RemoveRange(_context.Products);
            await _context.SaveChangesAsync();

            // Add test products
            var products = new[]
            {
                new Product { Name = "Widget", Price = 10.99m },
                new Product { Name = "Gadget", Price = 15.49m },
                new Product { Name = "Thingamajig", Price = 7.25m }
            };
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            // Add test orders
            var order1 = new Order
            {
                Items = new System.Collections.Generic.List<OrderItem>
                {
                    new OrderItem { Product = products[0], Quantity = 2 },
                    new OrderItem { Product = products[1], Quantity = 1 }
                }
            };
            var order2 = new Order
            {
                Items = new System.Collections.Generic.List<OrderItem>
                {
                    new OrderItem { Product = products[2], Quantity = 5 }
                }
            };
            _context.Orders.AddRange(order1, order2);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Database seeded with test data." });
        }
    }
}
