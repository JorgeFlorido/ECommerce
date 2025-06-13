using AutoMapper;
using ECommerce.API.Models.Requests.Product;
using ECommerce.Domain.Models;

namespace ECommerce.API.Mappers
{
  public class ProductMappingProfile : Profile
  {
    public ProductMappingProfile() 
    {
      CreateMap<AddProductRequest, Product>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.StockQuantity, opt => opt.Ignore());
    }
  }
}
