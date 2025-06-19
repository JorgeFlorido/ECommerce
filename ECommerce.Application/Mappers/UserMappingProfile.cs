using AutoMapper;
using ECommerce.Application.Requests.Commands.Users;
using ECommerce.Domain.Models.User;

namespace ECommerce.Application.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AddUserCommand, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
            CreateMap<UpdateUserCommand, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
} 