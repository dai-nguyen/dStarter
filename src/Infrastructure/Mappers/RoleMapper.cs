using AutoMapper;
using Infrastructure.Data;
using Shared.DTOs;

namespace Infrastructure.Mappers
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            CreateMap<AppRole, RoleDto>()
                .ReverseMap();
        }
    }
}
