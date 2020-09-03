using Infrastructure.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Specifications
{
    public class CustomAttributeSpecification : BaseSpecification<CustomAttribute>
    {
        public CustomAttributeSpecification(
            int[] definitionIds,
            int parentId,
            BaseFilterDto baseFilter = null, 
            Dictionary<string, Expression<Func<CustomAttribute, object>>> columnMaps = null) 
            : base(
                  _ => (definitionIds == null || definitionIds.Length == 0 || definitionIds.Contains(_.Id))
                  && _.ParentId == parentId, 
                  baseFilter, 
                  columnMaps)
        {
        }
    }
}
