using AutoMapper;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Order;

namespace ECommerce.Application.Mappers
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderItemCommand, OrderItem>()
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());
            // Shipping and billing addresses are mapped in handler due to discriminated union
        }
    }
} 