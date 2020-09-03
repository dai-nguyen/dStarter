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
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactDto>()
                .ReverseMap();
        }
    }

    public static class ContactMapper
    {
        public static Dictionary<string, Expression<Func<Contact, object>>> CustAttrDefColMaps =
            new Dictionary<string, Expression<Func<Contact, object>>>()
            {
                ["Id"] = _ => _.Id,
                ["FirstName"] = _ => _.FirstName,
                ["LastName"] = _ => _.LastName,
                ["Title"] = _ => _.Title,
                ["Email"] = _ => _.Email
            };

        public static ContactSpecification ToContactSpecification(this TableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

            var baseFilter = option.ToBaseFilterDtoSpecification();

            return new ContactSpecification(
                option.Search ?? "", "", "", "", "", null, baseFilter, CustAttrDefColMaps);


        }
    }
}
