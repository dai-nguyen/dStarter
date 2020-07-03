using AutoMapper;
using Infrastructure.Data;
using Shared.DTOs;

namespace Infrastructure.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<AppUser, UserDto>()
                .ReverseMap();
        }
    }
}
