using AutoMapper;
using ECommerce.Application.Requests.Commands.Addresses;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Mappers
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            CreateMap<AddCustomerAddressCommand, CustomerAddress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => new PostalCode(src.PostalCode, src.Country)));
            CreateMap<UpdateCustomerAddressCommand, CustomerAddress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => new PostalCode(src.PostalCode, src.Country)));
            CreateMap<CustomerAddressCommand, CustomerAddress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => new PostalCode(src.PostalCode, src.Country)));
            CreateMap<DeliveryPointAddressCommand, DeliveryPointAddress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => new PostalCode(src.PostalCode, src.Country)));
            CreateMap<LockerAddressCommand, LockerAddress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => new PostalCode(src.PostalCode, src.Country)));
        }
    }
} 