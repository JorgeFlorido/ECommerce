using AutoMapper;
using ECommerce.API.Models.Requests.Product;
using ECommerce.Application.Requests.Commands.Products;
using ECommerce.Application.Requests.Queries.Products;
using ECommerce.Domain.Common.Models;

namespace ECommerce.API.Mappers
{
  public class ProductMappingProfile : Profile
  {
    public ProductMappingProfile() 
    {
      CreateMap<AddProductRequest, AddProductCommand>();
      CreateMap<UpdateProductRequest, UpdateProductCommand>();
      CreateMap<GetAllProductsRequest, GetAllProductsQuery>();
      CreateMap<ProductFilterQuery, ProductFilterQuery>();
      CreateMap<PaginationQuery, PaginationQuery>();
    }
  }
}
