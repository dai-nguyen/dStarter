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
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketDto>();                
            CreateMap<TicketDto, Ticket>()                
                .ForMember(_ => _.Customer, opt => opt.Ignore())
                .ForMember(_ => _.Contact, opt => opt.Ignore());
        }
    }

    public static class TicketMapper
    {
        public static Dictionary<string, Expression<Func<Ticket, object>>> CustAttrDefColMaps =
            new Dictionary<string, Expression<Func<Ticket, object>>>()
            {
                ["Id"] = _ => _.Id,
                ["Title"] = _ => _.Title,
                ["Description"] = _ => _.Description,
                ["Status"] = _ => _.Status,
                ["IsBilled"] = _ => _.IsBilled,
                ["IsPaid"] = _ => _.IsPaid
            };

        public static TicketSpecification ToTicketSpecification(this TicketTableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

            var baseFilter = option.ToBaseFilterDtoSpecification();

            return new TicketSpecification(
                option.Search ?? "",
                option.Title, 
                option.Status, 
                option.IsBilled, 
                option.IsPaid, 
                option.ContactId,
                option.CustomerId, 
                baseFilter, 
                CustAttrDefColMaps);


        }
    }
}
