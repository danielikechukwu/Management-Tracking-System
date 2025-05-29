using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.DTOs
{
    public class TrackingCreateDTO
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string Carrier { get; set; }
        [Required]
        public DateTime EstimatedDeliveryDate { get; set; }
        [Required]
        public string TrackingNumber { get; set; }
    }
}
