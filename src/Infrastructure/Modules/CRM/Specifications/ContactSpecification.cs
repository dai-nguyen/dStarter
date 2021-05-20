using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Modules.CRM.Specifications
{
    public class ContactSpecification : BaseSpecification<Contact>
    {
        public ContactSpecification(
            string search = "",
            string title = "",
            string firstName = "",
            string lastName = "",
            string email = "",
            string customerId = null,
            BaseFilterDto baseFilter = null, 
            Dictionary<string, Expression<Func<Contact, object>>> columnMaps = null) 
            : base(
                  _ => (string.IsNullOrEmpty(search) || EF.Functions.ToTsVector("english",
                      _.FirstName + " " + _.LastName + " " + _.Email).Matches(search)) 
                  && (string.IsNullOrEmpty(title) || _.Title.StartsWith(title))
                  && (string.IsNullOrEmpty(firstName) || _.FirstName.StartsWith(firstName))
                  && (string.IsNullOrEmpty(lastName) || _.LastName.StartsWith(lastName))
                  && (string.IsNullOrEmpty(email) || _.Email.StartsWith(email))
                  && (string.IsNullOrEmpty(customerId) || _.CustomerId == customerId), 
                  baseFilter, 
                  columnMaps)
        {
        }
    }
}
