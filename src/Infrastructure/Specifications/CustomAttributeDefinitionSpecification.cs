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
            string objectName,
            string name,
            string displayName,
            BaseFilterDto baseFilter = null, 
            Dictionary<string, Expression<Func<CustomAttributeDefinition, object>>> columnMaps = null) 
            : base(
                  _ => (string.IsNullOrEmpty(objectName) || _.ObjectName == objectName)
                  && (string.IsNullOrEmpty(name) || _.Name == name)
                  && (string.IsNullOrEmpty(displayName) || _.DisplayName == displayName), 
                  baseFilter, 
                  columnMaps)
        {
        }
    }
}
