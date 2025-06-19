using AutoMapper;
using ECommerce.API.Models.Requests.Address;
using ECommerce.Application.Requests.Commands.Addresses;

namespace ECommerce.API.Mappers
{
  public class AddressMappingProfile : Profile
  {
    public AddressMappingProfile()
    {
      CreateMap<AddCustomerAddressRequest, AddCustomerAddressCommand>();
      CreateMap<UpdateCustomerAddressRequest, UpdateCustomerAddressCommand>();
    }
  }
} 