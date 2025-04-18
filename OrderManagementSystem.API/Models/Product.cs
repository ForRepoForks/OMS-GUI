namespace OrderManagementSystem.API.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a product in the order management system.
    /// Enforces domain validation for required fields and price constraints.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product name. Must not be empty.
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;

        private decimal price;

        /// <summary>
        /// Gets or sets the product price. Must be greater than zero.
        /// Throws <see cref="ArgumentException"/> if set to zero or negative.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price
        {
            get => price;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Price must be greater than zero.");
                price = value;
            }
        }

        /// <summary>
        /// Gets or sets the discount percentage for the product, if any.
        /// </summary>
        public decimal? DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the quantity threshold from which the discount is applied, if any.
        /// </summary>
        public int? DiscountQuantityThreshold { get; set; }
    }
}
