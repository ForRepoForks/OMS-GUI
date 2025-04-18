using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.API.Data;
using OrderManagementSystem.API.Models;

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
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(CreateProduct), new { id = product.Id }, product);
        }
    }
}
