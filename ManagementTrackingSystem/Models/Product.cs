using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.Models
{
    public class Product
    {
        public int Id { get; set; } // Primary Key

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountPercentage { get; set; } //Product Level Discount

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string SKU { get; set; } // Stock Keeping Unit

        [Required]
        public int StockQuantity { get; set; }

        public int CategoryId { get; set; } // Foreign Key

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
