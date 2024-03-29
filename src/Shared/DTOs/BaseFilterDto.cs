﻿using System;
using System.Collections.Generic;

namespace Shared.DTOs
{
    public class BaseFilterDto
    {
        public string[] IDs { get; set; } = null;
        public DateTime? DateCreated { get; set; } = null;
        public DateTime? DateUpdated { get; set; } = null;
        public string CreatedBy { get; set; } = "";
        public string UpdatedBy { get; set; } = "";
        public string ExternalId { get; set; } = "";

        public string SortBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public bool IsPagingEnabled { get; set; } = false;

        public BaseFilterDto()
        {
            IsPagingEnabled = PageSize > 0;
        }

        public BaseFilterDto(
            string[] ids,
            string sortBy,
            string sortDir,
            int pageNumber,
            int pageSize,
            bool isPagingEnabled)
        {
            IDs = ids;
            SortBy = sortBy;
            SortDirection = sortDir;
            PageNumber = pageNumber;
            PageSize = pageSize;
            IsPagingEnabled = isPagingEnabled;
        }
    }
}
