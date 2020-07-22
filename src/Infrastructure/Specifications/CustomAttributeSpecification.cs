using Infrastructure.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Specifications
{
    public class CustomAttributeSpecification : BaseSpecification<CustomAttribute>
    {
        public CustomAttributeSpecification(
            int? definitionId,
            BaseFilterDto baseFilter = null, 
            Dictionary<string, Expression<Func<CustomAttribute, object>>> columnMaps = null) 
            : base(
                  _ => (!definitionId.HasValue || _.DefinitionId == definitionId), 
                  baseFilter, 
                  columnMaps)
        {
        }
    }
}
