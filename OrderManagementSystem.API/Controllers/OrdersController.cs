using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.API.Data;
using OrderManagementSystem.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderManagementContext _context;

        public OrdersController(OrderManagementContext context)
        {
            _context = context;
        }

        public class CreateOrderRequest
        {
            [Required]
            [MinLength(1, ErrorMessage = "At least one item is required.")]
            public List<OrderItemDto> Items { get; set; } = new();
        }
        public class OrderItemDto
        {
            [Required]
            public int ProductId { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
            public int Quantity { get; set; }
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

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid || request.Items == null || request.Items.Count == 0)
                return BadRequest(ModelState);

            // Validate all product IDs
            var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
            if (products.Count != productIds.Count)
                return NotFound("One or more products not found.");

            // Map items
            var order = new Order();
            foreach (var item in request.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);
                order.Items.Add(new OrderItem { Product = product, ProductId = product.Id, Quantity = item.Quantity });
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var response = new OrderResponse
            {
                Id = order.Id,
                Items = order.Items.Select(i => new OrderItemResponse { ProductId = i.ProductId, Quantity = i.Quantity }).ToList()
            };
            return CreatedAtAction(nameof(CreateOrder), new { id = order.Id }, response);
        }
    }
}
