using ManagementTrackingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementTrackingSystem.Data
{
    public class ManagementTrackingSystemDBContext : DbContext
    {
        public ManagementTrackingSystemDBContext(DbContextOptions<ManagementTrackingSystemDBContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Membership Tiers
            modelBuilder.Entity<MembershipTier>().HasData();

            // Seed Customer Data
            modelBuilder.Entity<Customer>().HasData();

            // Seed Address Data
            modelBuilder.Entity<Address>().HasData();

            // Seed Category Data
            modelBuilder.Entity<Category>().HasData();

            // Seed Product Data
            modelBuilder.Entity<Product>().HasData();

            // Seed Order Data
            modelBuilder.Entity<Order>().HasData();

            // Seed OrderItem Data
            modelBuilder.Entity<OrderItem>().HasData();

            // Seed ShippingDetail Data
            modelBuilder.Entity<TrackingDetail>().HasData();


        }
    }
}
