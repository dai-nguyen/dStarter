using AutoMapper;
using Infrastructure.Mappers;
using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Modules.CRM.Specifications;
using Shared.DTOs.CRM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.CRM.Mappers
{
    public class LaborProfile : Profile
    {
        public LaborProfile()
        {
            CreateMap<Labor, LaborDto>();                
            CreateMap<LaborDto, Labor>()                                
                .ForMember(_ => _.Ticket, opt => opt.Ignore());
        }
    }

    public static class LaborMapper
    {
        public static Dictionary<string, Expression<Func<Labor, object>>> ColMaps =
            new Dictionary<string, Expression<Func<Labor, object>>>()
            {
                ["Id"] = _ => _.Id,
                ["Hour"] = _ => _.Hour,
                ["Minute"] = _ => _.Minute,
                ["Description"] = _ => _.Description                
            };

        public static LaborSpecification ToLaborSpecification(this LaborTableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

            var baseFilter = option.ToBaseFilterDtoSpecification();

            return new LaborSpecification(
                option.Search ?? "",                
                baseFilter, 
                ColMaps);


        }
    }
}
