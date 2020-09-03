using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Mappers
{
    public class CustomAttributeDefinitionProfile : Profile
    {
        public CustomAttributeDefinitionProfile()
        {
            CreateMap<CustomAttributeDefinition, CustomAttributeDefinitionDto>()
                .ReverseMap();
        }
    }

    public static class CustomAttributeDefinitionMapper
    {
        public static Dictionary<string, Expression<Func<CustomAttributeDefinition, object>>> CustAttrDefColMaps = 
            new Dictionary<string, Expression<Func<CustomAttributeDefinition, object>>>()
        {
                ["Id"] = _ => _.Id,
                ["ObjectName"] = _ => _.ObjectName,
                ["Name"] = _ => _.Name,
                ["DisplayName"] = _ => _.DisplayName,
                ["DataType"] = _ => _.DataType
        };

        public static CustomAttributeDefinitionSpecification ToCustomAttributeDefinitionSpecification(this TableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

            var baseFilter = option.ToBaseFilterDtoSpecification();

            return new CustomAttributeDefinitionSpecification(
                option.Search ?? "", "", baseFilter, CustAttrDefColMaps);


        }
    }
}
