using Infrastructure.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Specifications
{
    public class AppConfigSpecification : BaseSpecification<AppConfig>
    {
        public AppConfigSpecification(
            string key = "", 
            BaseFilterDto baseFilter = null, 
            Dictionary<string, Expression<Func<AppConfig, object>>> columnMaps = null) 
            : base(
                  (_ => string.IsNullOrEmpty(key) || _.Key == key), 
                  baseFilter, 
                  columnMaps)
        {
        }
    }
}
