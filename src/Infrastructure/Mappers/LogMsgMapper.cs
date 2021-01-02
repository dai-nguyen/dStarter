using Infrastructure.Specifications;
using Shared.DTOs;
using System;
using System.Linq;

namespace Infrastructure.Mappers
{
    public static class LogMsgMapper
    {
        public static LogMsgSpecification ToLogMsgSpecification(this LogMsgTableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("LogMsgTableOptionDto is required.");

            var sortBy = "Id";
            var sortDir = "asc";
            var pageNumber = option.Page;
            var pageSize = option.ItemsPerPage;
            var search = option.Search ?? "";

            if (option.SortBy != null
                && option.SortBy.Any()
                && option.SortDesc != null
                && option.SortDesc.Any())
            {
                sortBy = option.SortBy.First();
                sortDir = option.SortDesc.First() ? "desc" : "asc";
            }

            return new LogMsgSpecification(
                option.Username,
                option.Date,
                option.Levels,
                search, 
                sortBy, 
                sortDir, 
                pageSize, 
                pageNumber);
        }
    }
}
