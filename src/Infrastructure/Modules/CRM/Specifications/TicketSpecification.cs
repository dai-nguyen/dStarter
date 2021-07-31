using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.CRM.Specifications
{
    public class TicketSpecification : BaseSpecification<Ticket>
    {
        public TicketSpecification(
            string search = "",
            string title = "",
            string status = "",
            bool? isBilled = null,
            bool? isPaid = null,
            string customerId = null,
            string contactId = null,            
            BaseFilterDto baseFilter = null,
            Dictionary<string, Expression<Func<Ticket, object>>> columnMaps = null)
            : base(
                  _ => (string.IsNullOrEmpty(search) || EF.Functions.ToTsVector("english",
                      _.Title + " " + _.Description).Matches(search))
                  && (string.IsNullOrEmpty(title) || _.Title.StartsWith(title))
                  && (string.IsNullOrEmpty(status) || _.Status == status)
                  && (!isBilled.HasValue || _.IsBilled == isBilled.Value)
                  && (!isPaid.HasValue || _.IsPaid == isPaid.Value)
                  && (string.IsNullOrEmpty(customerId) || _.Contact.CustomerId == customerId)
                  && (string.IsNullOrEmpty(contactId) || _.ContactId == contactId),
                  baseFilter,
                  columnMaps)
        {
            if (!string.IsNullOrEmpty(customerId))
                Includes.Add(_ => _.Contact);
        }
    }
}
