﻿using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Specifications;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.CRM.Specifications
{
    public class TicketSpecification : BaseSpecification<Ticket>
    {
        public TicketSpecification(
            string title = "",
            string status = "",
            bool? isBilled = null,
            bool? isPaid = null,
            string contactId = null,
            string customerId = null,
            BaseFilterDto baseFilter = null,
            Dictionary<string, Expression<Func<Ticket, object>>> columnMaps = null)
            : base(
                  _ => (string.IsNullOrEmpty(title) || _.Title.StartsWith(title))
                  && (string.IsNullOrEmpty(status) || _.Status == status)
                  && (!isBilled.HasValue || _.IsBilled == isBilled.Value)
                  && (!isPaid.HasValue || _.IsPaid == isPaid.Value)
                  && (string.IsNullOrEmpty(contactId) || _.ContactId == contactId)
                  && (string.IsNullOrEmpty(customerId) || _.Contact.Customer.Id == customerId),
                  baseFilter,
                  columnMaps)
        {
        }
    }
}
