using AutoMapper;
using Infrastructure.Mappers;
using Infrastructure.Modules.Bank.Entities;
using Infrastructure.Modules.Bank.Specifications;
using Shared.DTOs.Bank;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.Bank.Mappers
{
    public class BankAccountTransactionProfile : Profile
    {
        public BankAccountTransactionProfile()
        {
            CreateMap<BankAccountTransaction, BankAccountTransactionDto>();
            CreateMap<BankAccountTransactionDto, BankAccountTransaction>()
                .ForMember(_ => _.BankAccount, opt => opt.Ignore());
        }
    }

    public static class BankAccountTransactionMapper
    {
        public static Dictionary<string, Expression<Func<BankAccountTransaction, object>>> ColMaps =
            new Dictionary<string, Expression<Func<BankAccountTransaction, object>>>()
            {
                ["Id"] = _ => _.Id,
                ["Name"] = _ => _.Name,
                ["Amount"] = _ => _.Amount,
                ["IsRealized"] = _ => _.IsRealized,                
            };

        public static BankAccountTransactionSpecification ToBankAccountTransactionSpecification(
            this BankAccountTransactionTableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

            var baseFilter = option.ToBaseFilterDtoSpecification();

            return new BankAccountTransactionSpecification(
                option.Search ?? "",
                option.UserId,
                option.BankAccountId,
                option.Name,
                option.Amount,
                option.IsRealized,
                baseFilter,
                ColMaps);
        }
    }
}
