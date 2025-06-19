using AutoMapper;
using ECommerce.Application.Requests.Commands.Products;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Mappers
{
  public class ProductMappingProfile : Profile
  {
    public ProductMappingProfile()
    {
      CreateMap<UpdateProductCommand, Product>();
      CreateMap<AddProductCommand, Product>();
    }
  }
}
