using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ManagementTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class Mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembershipTiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TierName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipTiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MembershipTierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_MembershipTiers_MembershipTierId",
                        column: x => x.MembershipTierId,
                        principalTable: "MembershipTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ShippingAddressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Carrier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Electronic Products Description", "Electronics" },
                    { 2, "Accessories Products Description", "Accessories" }
                });

            migrationBuilder.InsertData(
                table: "MembershipTiers",
                columns: new[] { "Id", "DiscountPercentage", "TierName" },
                values: new object[,]
                {
                    { 1, 15.0m, "Gold" },
                    { 2, 10.0m, "Silver" },
                    { 3, 5.0m, "Bronze" },
                    { 4, 0.0m, "Standard" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "IsActive", "LastName", "MembershipTierId", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(1985, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "pranayarout@example.com", "Pranaya", true, "Rout", 1, "1234567890" },
                    { 2, new DateTime(1988, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "hinasharma@example.com", "Hina", true, "Sharma", 2, "234567" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "DiscountPercentage", "Name", "Price", "SKU", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, "High-performance laptop", 10m, "Laptop", 1500m, "LPT-100", 50 },
                    { 2, 2, "Wireless mouse", 5m, "Mouse", 25m, "MSE-200", 200 },
                    { 3, 1, "Mechanical keyboard", 15m, "Keyboard", 50m, "KBD-300", 150 },
                    { 4, 1, "iPhone 15 pro", 0m, "Mobile", 2550m, "MOB-123", 100 }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "CustomerId", "Street", "ZipCode" },
                values: new object[,]
                {
                    { 1, "Jajpur", 1, "123 Main St", "755019" },
                    { 2, "Cuttack", 2, "456 Main St", "755123" },
                    { 3, "BBSR", 1, "789 Main St", "755456" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Amount", "CustomerId", "DeliveryCharge", "OrderDate", "OrderDiscount", "ShippedDate", "ShippingAddressId", "Status", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 1397.50m, 1, 0m, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 209.63m, null, 1, "Processing", 1187.87m },
                    { 2, 900m, 1, 50.0m, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 135m, null, 1, "Processing", 815.0m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "Discount", "OrderId", "ProductId", "ProductPrice", "Quantity", "TotalPrice" },
                values: new object[,]
                {
                    { 1, 150m, 1, 1, 1500m, 1, 1350m },
                    { 2, 2.50m, 1, 2, 25m, 2, 47.50m },
                    { 3, 12.5m, 2, 2, 25m, 10, 237.5m }
                });

            migrationBuilder.InsertData(
                table: "TrackingDetails",
                columns: new[] { "Id", "Carrier", "EstimatedDeliveryDate", "OrderId", "TrackingNumber" },
                values: new object[,]
                {
                    { 1, "FedEx", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "123456789" },
                    { 2, "FedEx", new DateTime(1988, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "123789456" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerId",
                table: "Addresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_MembershipTierId",
                table: "Customers",
                column: "MembershipTierId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingAddressId",
                table: "Orders",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDetails_OrderId",
                table: "TrackingDetails",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "TrackingDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "MembershipTiers");
        }
    }
}
