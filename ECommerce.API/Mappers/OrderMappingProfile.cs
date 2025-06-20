using AutoMapper;
using ECommerce.API.Models.Requests.Order;
using ECommerce.API.Models.Responses.Order;
using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Application.Models;

namespace ECommerce.API.Mappers
{
  public class OrderMappingProfile : Profile
  {
    public OrderMappingProfile()
    {
      CreateMap<OrderCostCalculationRequest, OrderCostCalculationQuery>();
      CreateMap<OrderCostCalculationResult, OrderCostCalculationResponse>()
        .ForMember(dest => dest.NetAmount, opt => opt.MapFrom(src => src.TotalAmount))
        .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
    }
  }
} 