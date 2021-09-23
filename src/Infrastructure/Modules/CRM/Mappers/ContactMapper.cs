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
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto, Contact>()
                .ForMember(_ => _.Customer, opt => opt.Ignore());
        }
    }

    public static class ContactMapper
    {
        public static Dictionary<string, Expression<Func<Contact, object>>> ColMaps =
            new Dictionary<string, Expression<Func<Contact, object>>>()
            {
                ["Id"] = _ => _.Id,
                ["FirstName"] = _ => _.FirstName,
                ["LastName"] = _ => _.LastName,
                ["Title"] = _ => _.Title,
                ["Email"] = _ => _.Email
            };

        public static ContactSpecification ToContactSpecification(this ContactTableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

            var baseFilter = option.ToBaseFilterDtoSpecification();
            
            return new ContactSpecification(
                option.Search ?? "",
                option.Title, 
                option.FirstName, 
                option.LastName, 
                option.Email, 
                option.CustomerId, 
                baseFilter, 
                ColMaps);
        }
    }
}
