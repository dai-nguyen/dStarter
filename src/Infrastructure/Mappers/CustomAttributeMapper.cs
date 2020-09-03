using AutoMapper;
using Infrastructure.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Mappers
{
    public class CustomAttributeMapperProfile : Profile
    {
        public CustomAttributeMapperProfile()
        {
            CreateMap<CustomAttribute, CustomAttributeDto>()
                .ReverseMap();
        }
    }

    public static class CustomAttributeMapper
    {
        public static Dictionary<string, Expression<Func<CustomAttribute, object>>> CustAttrColMaps =
            new Dictionary<string, Expression<Func<CustomAttribute, object>>>()
            {
                ["Id"] = _ => _.Id,
                ["DisplayName"] = _ => _.Definition.DisplayName,
                ["Value"] = _ => _.Value
            };
    }
}
