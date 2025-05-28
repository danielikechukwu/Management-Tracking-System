using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.Models
{
    public class OrderItem
    {
        public int Id { get; set; } //Primary Key

        [Required]
        public int OrderId { get; set; } //Foreign Key

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; } //Foreign Key

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductPrice { get; set; } //Product Prioce

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; } // Product Level Discount

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } //Product Level Total Price after Applying Discoun
    }
}
