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
                // Different property names: "Id" -> "OrderId"
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id));
                ;
        }
    }
}
