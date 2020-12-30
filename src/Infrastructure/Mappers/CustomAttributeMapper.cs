using AutoMapper;
using Infrastructure.Entities;
using Shared.DTOs;
using System;

namespace Infrastructure.Mappers
{
    public class CustomAttributeProfile : Profile
    {
        public CustomAttributeProfile()
        {
            CreateMap<CustomAttribute, CustomAttributeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<CustomAttributeDto, CustomAttribute>()
                .ForSourceMember(_ => _.Id, x => x.DoNotValidate());
        }
    }
}
