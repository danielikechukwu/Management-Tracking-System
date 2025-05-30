using AutoMapper;
using AutoMapper.QueryableExtensions;
using ManagementTrackingSystem.Data;
using ManagementTrackingSystem.DTOs;
using ManagementTrackingSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementTrackingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ManagementTrackingSystemDBContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        // The constructor injects:
        // 1. ECommerceDBContext for database access
        // 2. IMapper for AutoMapper
        // 3. IConfiguration for reading settings from appsettings.json

        public OrdersController(ManagementTrackingSystemDBContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        // ---------------------------
        // GET: api/Orders/GetOrderById/{id}
        // ---------------------------
        // This action method retrieves a single order by its ID and maps it
        // directly to an OrderDTO. We use AsNoTracking() since no updates are needed,
        // and ProjectTo<OrderDTO> to fetch only the columns required by the DTO.
        [HttpGet("GetOrderById/{    Id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById([FromRoute] int Id)
        {
            try
            {
                // Use AsNoTracking for a read-only query. 
                // Then, use ProjectTo<OrderDTO> to map from the Orders entity to 
                // our OrderDTO without manually specifying .Include() calls.
                var orderDTO = await _context.Orders
                    .AsNoTracking()
                    .Where(o => o.Id == Id)
                    .ProjectTo<OrderDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (orderDTO == null)
                    return NotFound($"Order with ID: {Id}");

                return Ok(orderDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the order: {ex.Message}");
            }
        }

        // ---------------------------
        // GET: api/Orders/GetOrdersByCustomerId/{customerId}
        // ---------------------------
        // This action method retrieves all orders for a specific customer.
        // Again, we use AsNoTracking() for a read-only scenario and ProjectTo 
        // to map directly to our DTO.
        [HttpGet("GetOrdersByCustomerId/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByCustomerId([FromRoute] int customerId)
        {
            try
            {
                // We filter by CustomerId, then project to OrderDTO and fetch the results
                var ordersDTO = await _context.Orders
                    .AsNoTracking()
                    .Where(o => o.CustomerId == customerId)
                    .ProjectTo<OrderDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                // If there are no matching orders, return 404
                if (!ordersDTO.Any())
                    return NotFound($"No orders found for customer with ID {customerId}");

                // Otherwise, return them with a 200 OK
                return Ok(ordersDTO);

            }
            catch (Exception ex)
            {
                // A catch-all for any runtime issues
                return StatusCode(500, $"An error occurred while fetching orders for customer {customerId}: {ex.Message}");
            }
        }

        // ---------------------------
        // POST: api/Orders/CreateOrder
        // ---------------------------
        // This endpoint creates a new order. It uses a transaction for atomicity 
        // and includes performance enhancements to fetch only necessary data.
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            // Validate incoming data quickly
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            else if (orderCreateDTO == null)
                return BadRequest("Order data cannot be null");

            // Start a transaction to ensure either all operations succeed or none do
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                // 1. Fetch the customer (and related membership + addresses) needed for discount & address validation
                //    We use AsNoTracking here because we don't want to update the customer's data 
                var customer = await _context.Customers
                    .AsNoTracking()
                    .Include(c => c.MembershipTier)
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.Id == orderCreateDTO.CustomerId && c.IsActive);

                // If the customer is invalid or not active, we stop here
                if (customer == null)
                    return NotFound($"Customer with ID {orderCreateDTO.CustomerId} not found or inactive");

                // 2. Verify the shipping address belongs to this customer
                var shippingAddress = customer.Addresses
                    .FirstOrDefault(a => a.Id == orderCreateDTO.ShippingAddressId);

                // If no matching address, we return a BadRequest
                if (shippingAddress == null)
                    return BadRequest("The specified shipping address is invalid or does not belong to the customer.");

                // 3. Extract the product IDs from the incoming DTO
                var productIds = orderCreateDTO.Items.Select(a => a.ProductId).ToList();

                // 4. Performance optimization: only fetch columns we actually need 
                //    (Price, DiscountPercentage, StockQuantity).
                var productData = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .Select(p => new
                    {
                        p.Id,
                        p.Price,
                        p.DiscountPercentage,
                        p.StockQuantity
                    }).ToListAsync();

                // If the requested productIDs don't match what's in the DB, some are invalid
                if (productData.Count != productIds.Count)
                    return BadRequest("One or more products in the order are invalid.");

                // 5. Ensure we have enough stock for each requested product
                foreach(var item in orderCreateDTO.Items)
                {
                    var productInfo = productData.FirstOrDefault(p => p.Id == item.ProductId);

                    if (productInfo == null || productInfo.StockQuantity < item.Quantity)
                    {
                        return BadRequest($"Insufficient stock for product ID {item.ProductId}." + $"Available: {productInfo?.StockQuantity ?? 0}");
                    }
                }

                // 6. Convert the OrderCreateDTO into an Order entity 
                //    (this sets basic fields like Status = "Pending" and Orderdate to DateTime.Now by default).
                var order = _mapper.Map<Order>(orderCreateDTO);

                // 7. Calculate the total amount of this order
                decimal totalAmount = 0;

                // We will need to update each order item's pricing details, and also 
                // reduce the product's stock in the database.
                foreach (var item in order.OrderItems)
                {
                    var productInfo = productData.First(p => p.Id == item.ProductId);

                    // Product-level discount
                    decimal productDiscount = productInfo.DiscountPercentage.HasValue ? 
                        productInfo.Price * productInfo.DiscountPercentage.Value / 100 : 0;

                    // Fill out the item details (price, discount, etc.)
                    item.ProductPrice = productInfo.Price;
                    item.Discount = productDiscount * item.Quantity;
                    item.TotalPrice = (productInfo.Price - productDiscount) * item.Quantity;

                    // Accumulate into totalAmount which will be sored in the order level
                    totalAmount += item.TotalPrice;

                    // Reduce stock in the actual tracked Product entity
                    // We do a separate fetch by ID so we can update the real entity
                    var trackedProduct = await _context.Products.FindAsync(item.ProductId);

                    if (trackedProduct == null)
                        return BadRequest($"Product with ID {item.ProductId} no longer exists.");

                    trackedProduct.StockQuantity -= item.Quantity;
                }

                // 8. Calculate membership discount for the entire order
                decimal membershipDiscountPercentage = customer.MembershipTier?.DiscountPercentage ?? 0m;

                decimal orderDiscount = totalAmount * (membershipDiscountPercentage / 100);

                // 9. Determine delivery charge based on config settings and totalAmount
                bool applyDeliveryCharge = _configuration.GetValue<bool>("DeliveryChargeSettings:ApplyDeliveryCharge");

                decimal deliveryChargeAmount = _configuration.GetValue<decimal>("DeliveryChargeSettings:DeliveryChargeAmount");

                decimal freeDeliveryThreshold = _configuration.GetValue<decimal>("DeliveryChargeSettings:FreeDeliveryThreshold");

                decimal deliveryCharge = 0;

                if (applyDeliveryCharge && totalAmount < freeDeliveryThreshold)
                {
                    deliveryCharge = deliveryChargeAmount;
                }

                // 10. Final calculation for Amount, OrderDiscount, DeliveryCharge, and TotalAmount for the Product Level
                order.Amount = totalAmount;

                order.OrderDiscount = orderDiscount;

                order.DeliveryCharge = deliveryCharge;

                order.TotalAmount = totalAmount - orderDiscount + deliveryCharge;

                // 11. Add the new order to the DB context for insertion
                _context.Orders.Add(order);

                // Save changes: includes new order and updated stock
                await _context.SaveChangesAsync();

                // Commit the transaction so everything is permanent
                await transaction.CommitAsync();

                // 12. Retrieve the newly created order in a read-only manner using AsNoTracking,
                //     then ProjectTo<OrderDTO> to create the final response object.
                var createdOrderDTO = await _context.Orders
                    .AsNoTracking()
                    .Where(o => o.Id == order.Id)
                    .ProjectTo<OrderDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                // If we can't find the newly-created order for some reason, throw 500
                if (createdOrderDTO == null)
                    return StatusCode(500, "An error occurred while creating the order.");

                // 13. Return a 200 OK response with the final OrderDTO
                return Ok(createdOrderDTO);

            }
            catch (Exception ex)
            {
                // Roll back the transaction if anything goes wrong
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred while creating the order: {ex.Message}");
            }
        }

    }
}
