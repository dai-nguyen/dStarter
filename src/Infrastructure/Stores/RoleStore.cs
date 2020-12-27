using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Stores
{
    public enum RoleKey
    {
        Id,
        Name
    }

    public class RoleStore : IRoleStore
    {
        protected readonly ILogger Logger;
        protected readonly RoleManager<AppRole> RoleManager;
        protected IMapper Mapper;
        protected IUserSession UserSession;

        protected readonly AppDbContext _dbContext;

        public RoleStore(
            ILogger<RoleStore> logger,
            RoleManager<AppRole> roleManager,
            IMapper mapper,
            IUserSession userSession,
            AppDbContext dbContext)
        {
            Logger = logger;
            RoleManager = roleManager;
            Mapper = mapper;
            UserSession = userSession;
            _dbContext = dbContext;
        }

        public virtual async Task<ActionResultDto<RoleDto>> AddAsync(RoleDto dto)
        {
            var action = new ActionResultDto<RoleDto>();

            try
            {
                var found = await RoleManager.FindByNameAsync(dto.Name);

                if (found != null)
                {
                    var err = $"Role already exists";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                var entity = Mapper.Map<AppRole>(dto);

                if (entity == null)
                {
                    var err = "Unable to map to role entity";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                if (string.IsNullOrEmpty(entity.Id))
                    entity.Id = Guid.NewGuid().ToString();

                var date = DateTime.Now;
                var username = UserSession.UserName;
                entity.DateCreated = date;
                entity.DateUpdated = date;
                entity.CreatedBy = username;
                entity.UpdatedBy = username;

                var created = await RoleManager.CreateAsync(entity);

                Logger.LogInformation($"Create role {dto.Name} - {created.Succeeded}");

                if (!created.Succeeded)
                {
                    Logger.LogError("Unable to create role {0} {@1} {UserName}",
                        dto.Name, created.Errors, UserSession.UserName);

                    if (created.Errors != null)
                    {
                        foreach (var err in created.Errors)
                        {
                            action.Errors.Add(err.Description);
                        }
                    }
                    return action;
                }

                action = await this.GetAsync(dto.Name, RoleKey.Name);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute AddAsync method {@0} {UserName}",
                    dto, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<PageDto<RoleDto>>> FindAsync(RoleSpecification spec)
        {
            var action = new ActionResultDto<PageDto<RoleDto>>();

            try
            {
                var query = RoleManager.Roles
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(spec.Search) && !string.IsNullOrWhiteSpace(spec.Search))
                {
                    query = query
                        .Where(_ => _.Name.Contains(spec.Search));
                }

                var colMaps = new Dictionary<string, Expression<Func<AppRole, object>>>()
                {
                    ["Id"] = _ => _.Id,
                    ["Name"] = _ => _.Name,
                    ["DateCreated"] = _ => _.DateCreated,
                    ["DateUpdated"] = _ => _.DateUpdated,
                    ["CreatedBy"] = _ => _.CreatedBy,
                    ["UpdatedBy"] = _ => _.UpdatedBy,
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

                action.Result = new PageDto<RoleDto>(total, Mapper.Map<RoleDto[]>(data));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute FindAsync method {@0} {UserName}",
                    spec, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<RoleDto>> GetAsync(string id, RoleKey key)
        {
            var action = new ActionResultDto<RoleDto>();

            try
            {
                AppRole entity = null;

                if (key == RoleKey.Id)
                    entity = await RoleManager.FindByIdAsync(id);
                else if (key == RoleKey.Name)
                    entity = await RoleManager.FindByNameAsync(id);

                if (entity == null)
                    return action;

                var dto = Mapper.Map<RoleDto>(entity);

                action.Result = dto;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute GetAsync method {@0} {UserName}",
                    new { id, key }, UserSession.UserName);
            }

            return action;
        }

        public virtual async Task<ActionResultDto<RoleDto>> UpdateAsync(RoleDto dto)
        {
            var action = new ActionResultDto<RoleDto>();

            try
            {
                var entity = await RoleManager.FindByIdAsync(dto.Id);

                if (entity == null)
                {
                    Logger.LogError($"Unable to find role {dto.Id}");
                    return action;
                }

                entity = Mapper.Map(dto, entity);

                if (entity == null)
                {
                    var err = "Unable to map to role entity";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                var date = DateTime.Now;
                var username = UserSession.UserName;
                entity.DateUpdated = date;
                entity.UpdatedBy = username;

                var updated = await RoleManager.UpdateAsync(entity);

                Logger.LogInformation($"Update role {dto.Name} - {updated.Succeeded}");

                if (!updated.Succeeded)
                {
                    Logger.LogError($"Unable to update Role {dto.Name}");
                    if (updated.Errors != null)
                    {
                        foreach (var err in updated.Errors)
                        {
                            action.Errors.Add(err.Description);
                        }
                    }
                    return action;
                }

                action = await this.GetAsync(dto.Name, RoleKey.Name);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute UpdateAsync method {@0} {UserName}",
                    dto, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<bool>> DeleteAsync(string id)
        {
            var action = new ActionResultDto<bool>() { Result = false };

            try
            {
                var role = await RoleManager.FindByIdAsync(id);

                if (role == null)
                {
                    Logger.LogError($"Unable to find Role {id}");
                    return action;
                }

                // remove claims
                var claims = await RoleManager.GetClaimsAsync(role);
                if (claims != null && claims.Any())
                {
                    foreach (var claim in claims)
                    {
                        await RoleManager.RemoveClaimAsync(role, claim);
                    }
                }

                // delete
                var result = await RoleManager.DeleteAsync(role);

                action.Result = result.Succeeded;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute DeleteAsync method {@0} {UserName}",
                    id, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<RoleDto>> GetByExternalIdAsync(string externalId)
        {
            var action = new ActionResultDto<RoleDto>();

            try
            {
                var entity = await RoleManager.Roles
                        .Where(_ => _.ExternalId == externalId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                action.Result = entity == null ? null : Mapper.Map<RoleDto>(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute GetByExternalIdAsync method {@0} {UserName}",
                    externalId, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<RoleDto[]>> ListAllAsync()
        {
            var action = new ActionResultDto<RoleDto[]>();

            try
            {
                var entity = await RoleManager.Roles
                        .AsNoTracking()
                        .ToListAsync();

                action.Result = entity == null ? null : Mapper.Map<RoleDto[]>(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute ListAllAsync method {UserName}",
                    UserSession.UserName);
            }
            return action;
        }

    }
}
