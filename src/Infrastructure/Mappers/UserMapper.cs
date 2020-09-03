using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Specifications;
using Microsoft.Extensions.Options;
using Shared.DTOs;
using System;
using System.Linq;

namespace Infrastructure.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<AppUser, UserDto>()
                .ReverseMap();
        }
    }

    public static class UserMapper
    {
        public static UserSpecification ToUserSpecification(this TableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("TableOption is required.");

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
                sortBy = option.SortBy.First().UppercaseFirst();
                sortDir = option.SortDesc.First() ? "desc" : "asc";
            }

            return new UserSpecification(search, sortBy, sortDir, pageSize, pageNumber);
        }
    }
}
