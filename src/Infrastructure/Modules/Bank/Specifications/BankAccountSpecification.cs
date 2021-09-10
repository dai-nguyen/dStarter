using Infrastructure.Modules.Bank.Entities;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.Bank.Specifications
{
    public class BankAccountSpecification : BaseSpecification<BankAccount>
    {
        public BankAccountSpecification(
            string search = null,
            string userId = null,
            string name = null,
            string type = null,
            BaseFilterDto baseFilter = null,
            Dictionary<string, Expression<Func<BankAccount, object>>> columnMaps = null)
            : base(
                  _ => (string.IsNullOrEmpty(search) || EF.Functions.ToTsVector("english",
                      _.Name + " " + _.Type).Matches(search))
                  && (string.IsNullOrEmpty(userId) || _.UserId == userId)
                  && (string.IsNullOrEmpty(name) || _.Name.StartsWith(name))                  
                  && (string.IsNullOrEmpty(type) || _.Type.StartsWith(type)),
                  baseFilter,
                  columnMaps)
        {
        }
    }
}
