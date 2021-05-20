using AutoMapper;
using Infrastructure.Mappers;
using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Modules.CRM.Specifications;
using Shared.DTOs;
using Shared.DTOs.CRM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.CRM.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>()
                .ReverseMap();
        }
    }

    public static class CustomerMapper
    {
        public static Dictionary<string, Expression<Func<Customer, object>>> CustomerColMaps =
            new Dictionary<string, Expression<Func<Customer, object>>>()
            {
                ["Id"] = _ => _.Id,
                ["Name"] = _ => _.Name,
                ["Address1"] = _ => _.Address1,
                ["City"] = _ => _.City,
                ["State"] = _ => _.State
            };

        public static CustomerSpecification ToCustomerSpecification(this CustomerTableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

            var baseFilter = option.ToBaseFilterDtoSpecification();

            return new CustomerSpecification(
                option.Search ?? "",
                option.Name ?? "",
                baseFilter, 
                CustomerColMaps);


        }
    }
}
