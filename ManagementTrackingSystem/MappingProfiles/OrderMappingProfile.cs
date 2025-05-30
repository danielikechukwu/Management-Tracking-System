using AutoMapper;
using ManagementTrackingSystem.DTOs;
using ManagementTrackingSystem.Models;

namespace ManagementTrackingSystem.MappingProfiles
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile() {

            //-----------------------------------------------------------------
            // 1. Order -> OrderDTO
            //-----------------------------------------------------------------
            CreateMap<Order, OrderDTO>()
                // Format "OrderDate" from DateTime to a string (e.g., '2025-02-06 13:15:00')
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate.ToString("yyyyy-MM-dd HH:mm:ss")))
                // Combine two properties into one
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
                // Map "Customer.PhoneNumber" -> "CustomerPhoneNumber"
                .ForMember(dest => dest.CustomerPhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
                // Map "ShippingAddres" (typo in entity) -> "ShippingAddress" (DTO)
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddres))
                // Format "ShippedDate" if not null
                .ForMember(dest => dest.ShippedDate, opt => opt.MapFrom(src => src.ShippedDate.HasValue ? src.ShippedDate.Value.ToString("yyyyy-MM-dd HH:mm:ss") : null))
                // Map "Customer.Email" -> "CustomerEmail" (different property names)
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
                // Different property names: "Id" -> "OrderId"
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id));

            // Note: We do NOT explicitly map .ForMember(dest => dest.TrackingDetail, ...)
            // or .ForMember(dest => dest.OrderItems, ...) here because:
            //  - The property names are the same in source and destination
            //  - We have separate mappings for TrackingDetail -> TrackingDetailDTO
            //    and OrderItem -> OrderItemDTO (see below)
            // AutoMapper will automatically use those mappings, as long as
            // the source and destination property names and types align.

            //-----------------------------------------------------------------
            // 2. OrderItem -> OrderItemDTO
            //-----------------------------------------------------------------
            CreateMap<OrderItem, OrderItemDTO>()
                // We only specify a custom mapping where source != destination.
                // "Product.Name" -> "ProductName" is a custom transformation.
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            // We do NOT specify mappings for ProductPrice or TotalPrice 
            // because the property names match exactly in both the source 
            // and the destination (and there's no special logic needed):
            //   Source: OrderItem.ProductPrice  -> Destination: OrderItemDTO.ProductPrice
            //   Source: OrderItem.TotalPrice    -> Destination: OrderItemDTO.TotalPrice
            // AutoMapper will do these by convention.

            //-----------------------------------------------------------------
            // 3. Address -> AddressDTO
            //-----------------------------------------------------------------
            // All property names match, and no special transform is needed,
            // so we don't need ForMember. This single CreateMap is enough.
            CreateMap<Address, AddressDTO>();

            //-----------------------------------------------------------------
            // 4. TrackingDetail -> TrackingDetailDTO
            //-----------------------------------------------------------------
            CreateMap<TrackingDetail, TrackingDetailDTO>()
                // We apply a null substitute for TrackingNumber if null.
                .ForMember(dest => dest.TrackingNumber, opt => opt.NullSubstitute("Tracking not available"));

            //-----------------------------------------------------------------
            // 5. OrderCreateDTO -> Order
            //-----------------------------------------------------------------
            CreateMap<OrderCreateDTO, Order>()
                // Set the OrderDate to the current time when creating a new order
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Now))
                // Initialize a default status (e.g., "Pending")
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
                // Map "OrderCreateDTO.OrderItems" -> "Order.Items" (different property names)
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));

            // We do NOT specify .ForMember for "Amount", "OrderDiscount", or "TotalAmount"
            // because they might be calculated logic in the controller/service layer
            // rather than mapped directly from the DTO.

            //-----------------------------------------------------------------
            // 6. OrderItemCreateDTO -> OrderItem
            //-----------------------------------------------------------------
            // Because the property names match ("ProductId", "Quantity") and
            // there's no special transformation, a default CreateMap is sufficient.
            CreateMap<OrderItemCreateDTO, OrderItem>();
        }
    }
}
