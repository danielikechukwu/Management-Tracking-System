using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.DTOs
{
    public class AddressDTO
    {
        [Required, MaxLength(200)]
        public string Street { get; set; }
        [Required, MaxLength(100)]
        public string City { get; set; }
        [Required, MaxLength(10)]
        public string ZipCode { get; set; }
    }
}
