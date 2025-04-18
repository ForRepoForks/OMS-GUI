namespace OrderManagementSystem.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public int? DiscountQuantityThreshold { get; set; }
    }
}
