using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.Models
{
    public class TrackingDetail
    {
        public int Id { get; set; } //Primary Key

        public int OrderId { get; set; } //Foreign Key

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public string Carrier { get; set; }

        public DateTime EstimatedDeliveryDate { get; set; }

        [MaxLength(500)]
        public string TrackingNumber { get; set; }
    }
}
