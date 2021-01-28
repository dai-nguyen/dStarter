using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Mappers
{
    public class AppConfigMapperProfile : Profile
    {
        public AppConfigMapperProfile()
        {
            CreateMap<AppConfig, AppConfigDto>()
                .ForMember(dest => dest.Value, 
                    opt => opt.MapFrom(src => src.Sensitive ? 
                        Helpers.Helper.DecryptString(src.Value, "Vrj5IBRznQOoNWmvmhsv1Q@#") : src.Value));

            CreateMap<AppConfigDto, AppConfig>()
                .ForMember(dest => dest.Value, 
                    opt => opt.MapFrom(src => src.Sensitive ? 
                        Helpers.Helper.EncryptString(src.Value, "Vrj5IBRznQOoNWmvmhsv1Q@#") : src.Value));
        }
        
    }

    public static class AppConfigMapper
    {
        public static AppConfigSpecification ToAppConfigSpecification(this AppConfigTableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("AppConfigTableOptionDto is required.");

            return new AppConfigSpecification(
                option.Key,
                option.ToBaseFilter(),
                new Dictionary<string, System.Linq.Expressions.Expression<Func<AppConfig, object>>>()
                {
                    ["name"] = _ => _.Description,
                    ["key"] = _ => _.Key,
                    ["value"] = _ => _.Value
                });
        }

        public static BaseFilterDto ToBaseFilter(this AppConfigTableOptionDto option)
        {
            var sortBy = "key";
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
