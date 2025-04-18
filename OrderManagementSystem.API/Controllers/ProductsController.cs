using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.API.Data;
using OrderManagementSystem.API.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly OrderManagementContext _context;

        public ProductsController(OrderManagementContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest(ModelState);
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(CreateProduct), new { id = product.Id }, product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? name)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }
            var products = await query.ToListAsync();
            return Ok(products);
        }

        [HttpPut("{id}/discount")]
        public async Task<ActionResult<Product>> ApplyDiscount(int id, [FromBody] DiscountDto discount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.DiscountPercentage = discount.Percentage;
            product.DiscountQuantityThreshold = discount.QuantityThreshold;
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        public class DiscountDto
        {
            [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
            public decimal Percentage { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Quantity threshold must be greater than 0.")]
            public int QuantityThreshold { get; set; }
        }
    }
}
