﻿using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Specifications;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.CRM.Specifications
{
    public class ContactSpecification : BaseSpecification<Contact>
    {
        public ContactSpecification(
            string title = "",
            string firstName = "",
            string lastName = "",
            string email = "",
            int? customerId = null,
            BaseFilterDto baseFilter = null, 
            Dictionary<string, Expression<Func<Contact, object>>> columnMaps = null) 
            : base(
                  _ => (string.IsNullOrEmpty(title) || _.Title.StartsWith(title))
                  && (string.IsNullOrEmpty(firstName) || _.FirstName.StartsWith(firstName))
                  && (string.IsNullOrEmpty(lastName) || _.LastName.StartsWith(lastName))
                  && (string.IsNullOrEmpty(email) || _.Email.StartsWith(email))
                  && (!customerId.HasValue || _.CustomerId == customerId.Value), 
                  baseFilter, 
                  columnMaps)
        {
        }
    }
}