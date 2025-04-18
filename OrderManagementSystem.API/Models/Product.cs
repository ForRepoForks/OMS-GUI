namespace OrderManagementSystem.API.Models
{
    using System.ComponentModel.DataAnnotations;

public class Product
    {
        public int Id { get; set; }
        [Required]
[MinLength(1)]
public string Name { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public int? DiscountQuantityThreshold { get; set; }
    }
}
