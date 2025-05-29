using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.DTOs
{
    public class OrderCreateDTO
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int ShippingAddressId { get; set; }
        [Required]
        public List<OrderItemCreateDTO> Items { get; set; }
    }
}
