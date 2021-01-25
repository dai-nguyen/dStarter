using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Specifications
{
    public class WikiSpecification : BaseSpecification<Wiki>
    {
        public WikiSpecification(
            string search = null,
            string tag = null,
            BaseFilterDto baseFilter = null,
            Dictionary<string, Expression<Func<Wiki, object>>> columnMaps = null
                
            ) : base (
                (_ => 
                    (string.IsNullOrEmpty(search) || EF.Functions.ToTsVector("english", 
                        _.Title + " " + _.Body).Matches(search))
                    && (string.IsNullOrEmpty(tag) || _.Tags.Contains(tag))
                ),
                baseFilter,
                columnMaps ?? new Dictionary<string, Expression<Func<Wiki, object>>>()
                {
                    ["id"] = _ => _.Id,
                    ["title"] = _ => _.Title,
                    ["body"] = _ => _.Body
                })
        {

        }
    }
}
