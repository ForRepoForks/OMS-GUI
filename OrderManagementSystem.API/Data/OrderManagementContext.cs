using Microsoft.EntityFrameworkCore;

namespace OrderManagementSystem.API.Data
{
    public class OrderManagementContext : DbContext
    {
        public OrderManagementContext(DbContextOptions<OrderManagementContext> options)
            : base(options)
        {
        }

        public DbSet<OrderManagementSystem.API.Models.Product> Products { get; set; } = null!;
    }
}
