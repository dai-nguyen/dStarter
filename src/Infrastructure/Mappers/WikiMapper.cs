using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappers
{
    public class WikiMapperProfile : Profile
    {
        public WikiMapperProfile()
        {
            CreateMap<Wiki, WikiDto>()
                .ReverseMap();
        }

    }

    public static class WikiMapper
    {
        public static WikiSpecification ToAppConfigSpecification(this WikiTableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("WikiTableOptionDto is required.");

            return new WikiSpecification(
                option.Search,
                option.Tag,
                option.ToBaseFilter(),
                new Dictionary<string, System.Linq.Expressions.Expression<Func<Wiki, object>>>()
                {
                    ["id"] = _ => _.Id,
                    ["title"] = _ => _.Title,
                    ["body"] = _ => _.Body
                });
        }

        public static BaseFilterDto ToBaseFilter(this WikiTableOptionDto option)
        {
            var sortBy = "title";
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

            return new BaseFilterDto(
                null,
                sortBy,
                sortDir,
                pageNumber,
                pageSize,
                true);
        }
    }
}
