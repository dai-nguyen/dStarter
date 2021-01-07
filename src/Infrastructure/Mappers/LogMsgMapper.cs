using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using Shared.DTOs;
using System;
using System.Linq;

namespace Infrastructure.Mappers
{
    public class LogMsgMapperProfile : Profile
    {
        public LogMsgMapperProfile()
        {
            CreateMap<LogMsg, LogMsgDto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }

    public static class LogMsgMapper
    {
        public static LogMsgSpecification ToLogMsgSpecification(this LogMsgTableOptionDto option)
        {
            if (option == null)
                throw new ArgumentNullException("LogMsgTableOptionDto is required.");

            var sortBy = "raise_date";
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
                if (sortBy == "raise_date_formatted")
                    sortBy = "raise_date";
                sortDir = option.SortDesc.First() ? "desc" : "asc";
            }

            return new LogMsgSpecification(
                option.Usernames,
                option.Date,
                option.LogLevels,
                search, 
                sortBy, 
                sortDir, 
                pageSize, 
                pageNumber);
        }
    }
}
