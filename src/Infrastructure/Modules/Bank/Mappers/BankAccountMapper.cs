using AutoMapper;
using Infrastructure.Modules.Bank.Entities;
using Infrastructure.Modules.Bank.Specifications;
using Shared.DTOs.Bank;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.Bank.Mappers
{
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<BankAccount, BankAccountDto>();
        }
    }

    public static class BankAccountMapper
    {
        public static Dictionary<string, Expression<Func<BankAccount, object>>> ColMaps =
            new Dictionary<string, Expression<Func<BankAccount, object>>>()
            {
                ["Id"] = _ => _.Id,
                ["Name"] = _ => _.Name,
                ["Type"] = _ => _.Type
            };

        public static BankAccountSpecification ToBankAccountSpecification(this BankAccountTableOptionDto option)
        {

        }
    }
}
