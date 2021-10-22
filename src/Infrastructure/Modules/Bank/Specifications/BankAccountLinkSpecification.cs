using Infrastructure.Modules.Bank.Entities;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.Bank.Specifications
{
    public class BankAccountLinkSpecification : BaseSpecification<BankAccountLink>
    {
        public BankAccountLinkSpecification(
           string search = null,
           string userId = null,
           string bankAccountId = null,
           string name = null,           
           BaseFilterDto baseFilter = null,
           Dictionary<string, Expression<Func<BankAccountLink, object>>> columnMaps = null)
           : base(
                 _ => (string.IsNullOrEmpty(search) || EF.Functions.ToTsVector("english",
                     _.Name).Matches(search))
                 && (string.IsNullOrEmpty(userId) || _.UserId == userId)
                 && (string.IsNullOrEmpty(bankAccountId) || _.BankAccountId == bankAccountId)
                 && (string.IsNullOrEmpty(name) || _.Name.StartsWith(name)),
                 baseFilter,
                 columnMaps)
        {
        }
    }
}
