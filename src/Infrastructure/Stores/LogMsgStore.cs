using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Stores
{
    public class LogMsgStore : ILogMsgStore
    {
        protected readonly ILogger<LogMsgStore> Logger;
        protected IMapper Mapper;
        protected IUserSession UserSession;

        protected readonly AppDbContext _dbContext;

        public LogMsgStore(
            ILogger<LogMsgStore> logger,
            IMapper mapper,
            IUserSession userSession,
            AppDbContext dbContext)
        {
            Logger = logger;
            Mapper = mapper;
            UserSession = userSession;
            _dbContext = dbContext;
        }

        public virtual async Task<int> DeleteAsync(DateTime date)
        {
            //var sql = $"delete from public.\"Logs\" where raise_date <= '{date.ToString("yyyyMMdd")}'";
            var sql = "delete from public.\"Logs\" where raise_date <= {0}";
            try
            { 
                return await _dbContext.Database.ExecuteSqlRawAsync(sql, date);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute DeleteAsync {@0} {UserName}",
                    sql, UserSession.UserName);
            }
            return -1;
        }

        public virtual async Task<ActionResultDto<PageDto<LogMsgDto>>> FindAsync(LogMsgSpecification spec)
        {
            var action = new ActionResultDto<PageDto<LogMsgDto>>();

            try
            {
                Logger.LogInformation("spec {@0} {UserName}",
                    spec, UserSession.UserName);

                var query = _dbContext.Logs
                    .AsNoTracking()
                    .AsQueryable();

                var userNames = new List<string>();

                if (UserSession.Roles.Contains("Admin")
                    && spec.UserNames != null
                    && spec.UserNames.Any())
                {
                    userNames.AddRange(spec.UserNames.Select(_ => $"\"{_}\"").ToArray());
                }
                else if (!UserSession.Roles.Contains("Admin"))
                {
                    userNames.Add($"\"{UserSession.UserName}\"");
                }

                if (userNames.Contains("Unknown"))
                {
                    query = query
                        .Where(_ => userNames.Contains(_.user_name) || _.user_name == null);
                }
                else
                {
                    query = query
                        .Where(_ => userNames.Contains(_.user_name));
                }

                query = query
                    .Where(_ => _.raise_date >= spec.Date.Date 
                        && _.raise_date <= spec.Date.Date.AddHours(24));

                if (spec.Levels != null && spec.Levels.Any())
                {
                    query = query
                        .Where(_ => spec.Levels.Contains(_.level));
                }

                if (!string.IsNullOrEmpty(spec.Search) 
                    && !string.IsNullOrWhiteSpace(spec.Search))
                {
                    query = query
                        .Where(_ => EF.Functions.ToTsVector("english",
                            _.message + " " + _.exception)
                                .Matches(spec.Search));
                }

                var colMaps = new Dictionary<string, Expression<Func<LogMsg, object>>>()
                {
                    ["message"] = _ => _.message,
                    ["message_template"] = _ => _.message_template,
                    ["level"] = _ => _.level,
                    ["raise_date"] = _ => _.raise_date,
                    ["exception"] = _ => _.exception,
                    ["properties"] = _ => _.properties,
                    ["machine_name"] = _ => _.machine_name,
                    ["user_name"] = _ => _.user_name
                };

                if (!string.IsNullOrEmpty(spec.SortBy)
                    && !string.IsNullOrEmpty(spec.SortDirection)
                    && colMaps.ContainsKey(spec.SortBy))
                {
                    if (spec.SortDirection.Equals("asc", StringComparison.CurrentCultureIgnoreCase))
                    {
                        query = query.OrderBy(colMaps[spec.SortBy]);
                    }
                    else if (spec.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                    {
                        query = query.OrderByDescending(colMaps[spec.SortBy]);
                    }
                }

                long total = await query.LongCountAsync();

                // Apply paging if enabled
                if (spec.PageSize > 0 && spec.PageNumber > 0)
                {
                    int skip = (spec.PageNumber - 1) * spec.PageSize;

                    query = query.Skip(skip)
                                 .Take(spec.PageSize);
                }

                var data = await query
                    .ToListAsync();

                action.Result = new PageDto<LogMsgDto>(total, Mapper.Map<LogMsgDto[]>(data));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute FindAsync method {@0} {UserName}",
                    spec, UserSession.UserName);
            }
            return action;
        }
    }
}
