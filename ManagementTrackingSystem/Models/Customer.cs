using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ManagementTrackingSystem.Models
{
    public class Customer
    {
        public int Id { get; set; } //Primary Key

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsActive { get; set; }

        public int MembershipTierId { get; set; } // Foreign key to MembershipTier

        [ForeignKey("MembershipTierId")]
        public MembershipTier MembershipTier { get; set; }

        public List<Address> Addresses { get; set; }

        public List<Order> Orders { get; set; }
    }
}
