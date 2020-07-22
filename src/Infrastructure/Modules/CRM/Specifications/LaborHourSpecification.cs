using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Specifications;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.CRM.Specifications
{
    public class LaborHourSpecification : BaseSpecification<LaborHour>
    {
        public LaborHourSpecification(
            int? ticketId = null, 
            BaseFilterDto baseFilter = null, 
            Dictionary<string, Expression<Func<LaborHour, object>>> columnMaps = null) 
            : base(
                  _ => (!ticketId.HasValue || _.TicketId == ticketId), 
                  baseFilter, 
                  columnMaps)
        {
        }
    }
}
