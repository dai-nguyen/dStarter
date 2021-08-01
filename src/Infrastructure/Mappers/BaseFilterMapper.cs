using Infrastructure.Helpers;
using Shared.DTOs;
using System;
using System.Linq;

namespace Infrastructure.Mappers
{
    public static class BaseFilterMapper
    {
        public static BaseFilterDto ToBaseFilterDtoSpecification(this TableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

            var result = new BaseFilterDto();

            result.PageSize = option.ItemsPerPage;
            result.PageNumber = option.Page;

            result.IsPagingEnabled = option.ItemsPerPage > 0;

            if (option.SortBy != null
                && option.SortBy.Any()
                && option.SortDesc != null
                && option.SortDesc.Any())
            {
                result.SortBy = option.SortBy.First().UppercaseFirst();
                result.SortDirection = option.SortDesc.First() ? "desc" : "asc";
            }

            return result;
        }
    }
}
