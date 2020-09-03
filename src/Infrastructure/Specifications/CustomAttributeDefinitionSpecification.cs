using Infrastructure.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Specifications
{
    public class CustomAttributeDefinitionSpecification : BaseSpecification<CustomAttributeDefinition>
    {
        public CustomAttributeDefinitionSpecification(
            string search,
            string objectName,
            BaseFilterDto baseFilter = null, 
            Dictionary<string, Expression<Func<CustomAttributeDefinition, object>>> columnMaps = null) 
            : base(
                  _ => (string.IsNullOrEmpty(search) || _.ObjectName == search || _.Name == search || _.DisplayName == search)
                  && (string.IsNullOrEmpty(objectName) || _.ObjectName == objectName),
                  baseFilter, 
                  columnMaps)
        {
        }
    }
}
