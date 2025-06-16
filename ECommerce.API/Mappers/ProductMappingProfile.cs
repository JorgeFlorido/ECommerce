using AutoMapper;
using ECommerce.API.Models.Requests.Product;
using ECommerce.Application.Requests.Commands.Products;

namespace ECommerce.API.Mappers
{
  public class ProductMappingProfile : Profile
  {
    public ProductMappingProfile() 
    {
      CreateMap<AddProductRequest, AddProductCommand>();
    }
  }
}
