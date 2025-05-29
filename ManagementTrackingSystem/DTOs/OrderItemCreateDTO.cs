using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.DTOs
{
    public class OrderItemCreateDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
