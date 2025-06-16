using AutoMapper;
using ECommerce.Application.Requests.Commands.Products;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
