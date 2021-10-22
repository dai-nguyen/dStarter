using AutoMapper;
using Infrastructure.Modules.Bank.Entities;
using Shared.DTOs.Bank;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.Bank.Mappers
{
    public class BankAccountLinkProfile : Profile
    {
        public BankAccountLinkProfile()
        {
            CreateMap<BankAccountLink, BankAccountLinkDto>();
            CreateMap<BankAccountLinkDto, BankAccountLink>()
                .ForMember(_ => _.BankAccount, opt => opt.Ignore());
        }
    }

    public static class BankAccountLinkMapper
    {
        public static Dictionary<string, Expression<Func<BankAccountLink, object>>> ColMaps =
            new Dictionary<string, Expression<Func<BankAccountLink, object>>>()
            {
                ["Id"] = _ => _.Id,
                ["Name"] = _ => _.Name,
                ["UserId"] = _ => _.UserId,
                ["BankAccountId"] = _ => _.BankAccountId,                
            };
    }
}
