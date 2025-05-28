using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.Models
{
    public class Address
    {
        public int Id { get; set; } //Primary Key

        [Required, MaxLength(200)]
        public string Street { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(10)]
        public string ZipCode { get; set; }

        public int CustomerId { get; set; } //Foreign Key

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } // Navigation property for the Customer
    }
}
