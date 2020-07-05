using Infrastructure.Helpers;
using Infrastructure.Specifications;
using System;
using System.Linq;
using Web.Models;

namespace Web.Mappers
{
    public static class RoleMapper
    {
        public static RoleSpecification ToRoleSpecification(this TableOption option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

            var sortBy = "Id";
            var sortDir = "asc";
            var pageNumber = 1;
            var pageSize = 15;

            if (option.SortBy != null 
                && option.SortBy.Any() 
                && option.SortDesc != null 
                && option.SortDesc.Any())
            {
                sortBy = option.SortBy.First().UppercaseFirst();
                sortDir = option.SortDesc.First() ? "desc" : "asc";

                if (sortBy == "DateUpdatedFormatted")
                    sortBy = "DateUpdated";
            }

            return new RoleSpecification("", sortBy, sortDir, pageSize, pageNumber);
        }
    }
}
