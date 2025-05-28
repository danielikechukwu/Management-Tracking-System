using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.Models
{
    public class Order
    {
        public int Id { get; set; } //Primary Key

        [Required]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } //Base Amount

        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderDiscount { get; set; } //Order Level Discount

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryCharge { get; set; } //Delivery Charge

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } //Total Amount After Apply Discount and Delivery Charge

        [MaxLength(20)]
        public string Status { get; set; }

        public DateTime? ShippedDate { get; set; }

        [Required]
        public int? CustomerId { get; set; } //Foreign Key

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public int? ShippingAddressId { get; set; } //Foreign Key

        [ForeignKey("ShippingAddressId")]
        public Address ShippingAddres { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public TrackingDetail TrackingDetail { get; set; }
    }
}
