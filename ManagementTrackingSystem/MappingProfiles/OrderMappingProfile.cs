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
        }
    }
}
