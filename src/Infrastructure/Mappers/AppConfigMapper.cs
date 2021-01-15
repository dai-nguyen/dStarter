using AutoMapper;
using Infrastructure.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappers
{
    public class AppConfigMapperProfile : Profile
    {
        public AppConfigMapperProfile()
        {
            CreateMap<AppConfig, AppConfigDto>()
                .ReverseMap();
        }
        
    }

    public static class AppConfigMapper
    {
    }
}
