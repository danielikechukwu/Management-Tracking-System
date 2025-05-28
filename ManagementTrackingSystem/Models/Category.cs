using System.ComponentModel.DataAnnotations;

namespace ManagementTrackingSystem.Models
{
    public class Category
    {
        public int Id { get; set; } // Primary Key

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public List<Product> Products { get; set; }
    }
}
