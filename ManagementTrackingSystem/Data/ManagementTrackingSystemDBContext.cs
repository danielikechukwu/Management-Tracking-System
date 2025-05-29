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
            modelBuilder.Entity<MembershipTier>().HasData(
                new MembershipTier { Id = 1, TierName = "Gold", DiscountPercentage = 15.0m  },
                new MembershipTier { Id = 2, TierName = "Silver", DiscountPercentage = 10.0m },
                new MembershipTier { Id = 3, TierName = "Bronze", DiscountPercentage = 5.0m },
                new MembershipTier { Id = 4, TierName = "Standard", DiscountPercentage = 0.0m }
                );

            // Seed Customer Data
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "Pranaya",
                    LastName = "Rout",
                    Email = "pranayarout@example.com",
                    PhoneNumber = "1234567890",
                    DateOfBirth = new DateTime(1985, 5, 20),
                    IsActive = true,
                    MembershipTierId = 1 // Gold Member
                },
                new Customer {
                    Id = 2,
                    FirstName = "Hina",
                    LastName = "Sharma",
                    Email = "hinasharma@example.com",
                    PhoneNumber = "234567",
                    DateOfBirth = new DateTime(1988, 8, 15),
                    IsActive = true,
                    MembershipTierId = 2 // Silver Member
                }
                );

            // Seed Address Data
            modelBuilder.Entity<Address>().HasData(
                new Address { Id = 1, Street = "123 Main St", City = "Jajpur", ZipCode = "755019", CustomerId = 1 },
                new Address { Id = 2, Street = "456 Main St", City = "Cuttack", ZipCode = "755123", CustomerId = 2 },
                new Address { Id = 3, Street = "789 Main St", City = "BBSR", ZipCode = "755456", CustomerId = 1 }
                );

            // Seed Category Data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic Products Description" },
                new Category { Id = 2, Name = "Accessories", Description = "Accessories Products Description" }
                );

            // Seed Product Data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 1500m, Description = "High-performance laptop", SKU = "LPT-100", StockQuantity = 50, CategoryId = 1, DiscountPercentage = 10 },
                new Product { Id = 2, Name = "Mouse", Price = 25m, Description = "Wireless mouse", SKU = "MSE-200", StockQuantity = 200, CategoryId = 2, DiscountPercentage = 5 },
                new Product { Id = 3, Name = "Keyboard", Price = 50m, Description = "Mechanical keyboard", SKU = "KBD-300", StockQuantity = 150, CategoryId = 1, DiscountPercentage = 15 },
                new Product { Id = 4, Name = "Mobile", Price = 2550m, Description = "iPhone 15 pro", SKU = "MOB-123", StockQuantity = 100, CategoryId = 1, DiscountPercentage = 0 }
                );

            // Seed Order Data
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    OrderDate = new DateTime(2025, 02, 06),
                    CustomerId = 1,
                    ShippingAddressId = 1,
                    Status = "Processing",
                    Amount = 1397.50m,  // Total amount before applying membership discount
                    OrderDiscount = 209.63m, // Membership discount based on Gold membership (15%)
                    DeliveryCharge = 0m, // Delivery charge waived
                    TotalAmount = 1187.87m // Final total after all discounts
                },
                new Order
                {
                    Id = 2,
                    OrderDate = new DateTime(2025, 02, 05),
                    CustomerId = 1,
                    ShippingAddressId = 1,
                    Status = "Processing",
                    Amount = 900m, // Total amount before discounts and charges
                    OrderDiscount = 135m, // 15% Gold membership discount
                    DeliveryCharge = 50.0m, // Delivery charge applied
                    TotalAmount = 900m - 135m + 50.0m, // Total after discount and delivery charge
                }
                );

            // Seed OrderItem Data
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 1,
                    ProductPrice = 1500m, //Actual Product Price
                    Discount = 150m, // Product-level discount for Laptop (10%)
                    TotalPrice = 1350m // Price after product discount
                },
                new OrderItem
                {
                    Id = 2,
                    OrderId = 1,
                    ProductId = 2,
                    Quantity = 2,
                    ProductPrice = 25m, //Actual Product Price
                    Discount = 2.50m, // Product-level discount for 2 Mice (5% each, total 2.5)
                    TotalPrice = 47.50m // Price after product discount
                },
                new OrderItem
                {
                    Id = 3,
                    OrderId = 2,
                    ProductId = 2,
                    Quantity = 10,
                    ProductPrice = 25m, //Actual Product Price
                    Discount = 12.5m, // Product-level discount (5% of 25 * 10)
                    TotalPrice = (25m * 10) - 12.5m // Total price after discount
                }
                );

            // Seed ShippingDetail Data
            modelBuilder.Entity<TrackingDetail>().HasData(
                new TrackingDetail { Id = 1, OrderId = 1, Carrier = "FedEx", EstimatedDeliveryDate = new DateTime(2025, 02, 15), TrackingNumber = "123456789" },
                new TrackingDetail { Id = 2, OrderId = 2, Carrier = "FedEx", EstimatedDeliveryDate = new DateTime(1988, 02, 15), TrackingNumber = "123789456" }
                );
        }

        public DbSet<MembershipTier> MembershipTiers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<TrackingDetail> TrackingDetails { get; set; }
    }
}
