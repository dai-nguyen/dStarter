using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.CRM.Specifications
{
    public class LaborSpecification : BaseSpecification<Labor>
    {
        public LaborSpecification(
            string search = "",            
            BaseFilterDto baseFilter = null,
            Dictionary<string, Expression<Func<Labor, object>>> columnMaps = null)
            : base(
                  _ => (string.IsNullOrEmpty(search) || EF.Functions.ToTsVector("english",
                      _.Description).Matches(search)),                  
                  baseFilter,
                  columnMaps)
        {
        }
    }
}
