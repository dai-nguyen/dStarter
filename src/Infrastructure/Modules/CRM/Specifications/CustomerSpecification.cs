using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.CRM.Specifications
{
    public class CustomerSpecification : BaseSpecification<Customer>
    {
        public CustomerSpecification(
            string search = "",
            string name = "",
            BaseFilterDto baseFilter = null,
            Dictionary<string, Expression<Func<Customer, object>>> columnMaps = null)
            : base(
                  _ => (string.IsNullOrEmpty(search) || EF.Functions.ToTsVector("english", _.Name).Matches(search))
                  && (string.IsNullOrEmpty(name) || _.Name.Contains(name)),
                  baseFilter,
                  columnMaps)
        {
        }
    }
}
