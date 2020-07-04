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
    public enum UserKey
    {
        Id,
        UserName,
        Email
    }

    public class UserStore : IUserStore
    {
        protected readonly ILogger Logger;
        protected readonly UserManager<AppUser> UserManager;
        protected IMapper Mapper;
        protected IUserSession UserSession;

        protected readonly AppDbContext _dbContext;

        public UserStore(
           ILogger<UserStore> logger,
           UserManager<AppUser> userManager,
           IMapper mapper,
           IUserSession userSession,
           AppDbContext dbContext)
        {
            Logger = logger;
            UserManager = userManager;
            Mapper = mapper;
            UserSession = userSession;
            _dbContext = dbContext;
        }

        public virtual async Task<ActionResultDto<UserDto>> AddAsync(UserDto dto)
        {
            var action = new ActionResultDto<UserDto>();

            try
            {
                var found = await UserManager.FindByNameAsync(dto.UserName);

                if (found != null)
                {
                    var err = $"Username already exists";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                found = await UserManager.FindByEmailAsync(dto.Email);

                if (found != null)
                {
                    var err = $"Email already exists";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                var entity = Mapper.Map<AppUser>(dto);

                if (entity == null)
                {
                    var err = "Unable to map to user entity";
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

                var created = await UserManager.CreateAsync(entity, dto.Password);

                Logger.LogInformation($"Create user {dto.Email} - {created.Succeeded}");

                if (!created.Succeeded)
                {
                    Logger.LogError("Unable to create user {0} {@1} {UserName}",
                        dto.UserName, created.Errors, UserSession.UserName);

                    if (created.Errors != null)
                    {
                        foreach (var err in created.Errors)
                        {
                            action.Errors.Add(err.Description);
                        }
                    }
                    return action;
                }

                await UpsertAttributesAsync(entity, dto);
                await UpsertRolesAsync(entity, dto);

                action = await this.GetAsync(dto.UserName, UserKey.UserName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute AddAsync method {@0} {UserName}",
                    dto, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<PageDto<UserDto>>> FindAsync(UserSpecification spec)
        {
            var action = new ActionResultDto<PageDto<UserDto>>();

            try
            {
                var query = UserManager.Users
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(spec.Search) && !string.IsNullOrWhiteSpace(spec.Search))
                {
                    query = query
                        .Where(_ => _.FirstName.Contains(spec.Search)
                            || _.LastName.Contains(spec.Search)
                            || _.Email.Contains(spec.Search));
                }

                var colMaps = new Dictionary<string, Expression<Func<AppUser, object>>>()
                {
                    ["Id"] = _ => _.Id,
                    ["UserName"] = _ => _.UserName,
                    ["Email"] = _ => _.Email,
                    ["FirstName"] = _ => _.FirstName,
                    ["LastName"] = _ => _.LastName,
                    ["ExternalId"] = _ => _.ExternalId,
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

                action.Result = new PageDto<UserDto>(total, Mapper.Map<UserDto[]>(data));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute FindAsync method {@0} {UserName}", 
                    spec, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<UserDto>> GetAsync(string id, UserKey key)
        {
            var action = new ActionResultDto<UserDto>();

            try
            {
                AppUser entity = null;

                if (key == UserKey.Email)
                    entity = await UserManager.FindByEmailAsync(id);
                else if (key == UserKey.Id)
                    entity = await UserManager.FindByIdAsync(id);
                else if (key == UserKey.UserName)
                    entity = await UserManager.FindByNameAsync(id);

                if (entity == null)
                    return action;

                var dto = Mapper.Map<UserDto>(entity);

                var attrs = await _dbContext.UserAttributes
                    .AsNoTracking()
                    .Where(_ => _.UserId == entity.Id)
                    .ToListAsync();

                if (attrs != null)
                {
                    dto.Attributes = attrs
                        .Select(_ => new UserAttributeDto()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = _.Type,
                            Value = _.Value
                        }).ToArray();
                }

                var roles = await UserManager.GetRolesAsync(entity);

                if (roles != null)
                {
                    dto.Roles = roles;
                }

                action.Result = dto;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute GetAsync method {@0} {UserName}", 
                    new { id, key }, UserSession.UserName);
            }

            return action;
        }

        public virtual async Task<ActionResultDto<UserDto>> UpdateAsync(UserDto dto)
        {
            var action = new ActionResultDto<UserDto>();

            try
            {
                var entity = await UserManager.FindByIdAsync(dto.Id);

                if (entity == null)
                {
                    Logger.LogError($"Unable to find user {dto.Id}");
                    return action;
                }

                entity = Mapper.Map(dto, entity);

                if (entity == null)
                {
                    var err = "Unable to map to user entity";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                var date = DateTime.Now;
                var username = UserSession.UserName;
                entity.DateUpdated = date;
                entity.UpdatedBy = username;

                var updated = await UserManager.UpdateAsync(entity);

                Logger.LogInformation($"Update user {dto.Email} - {updated.Succeeded}");

                if (!updated.Succeeded)
                {
                    Logger.LogError($"Unable to update User {dto.UserName}");
                    if (updated.Errors != null)
                    {
                        foreach (var err in updated.Errors)
                        {
                            action.Errors.Add(err.Description);
                        }
                    }
                    return action;
                }

                if (!string.IsNullOrEmpty(dto.Password))
                {
                    var token = await UserManager.GeneratePasswordResetTokenAsync(entity);
                    var passReset = await UserManager.ResetPasswordAsync(entity, token, dto.Password);
                    Logger.LogInformation($"Reset user {dto.Email} password - {passReset.Succeeded}");
                }

                await UpsertAttributesAsync(entity, dto);
                await UpsertRolesAsync(entity, dto);

                action = await this.GetAsync(dto.UserName, UserKey.UserName);
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
                var user = await UserManager.FindByIdAsync(id);

                if (user == null)
                {
                    Logger.LogError($"Unable to find user {id}");
                    return action;
                }

                // remove claims
                var claims = await UserManager.GetClaimsAsync(user);
                if (claims != null && claims.Any())
                    await UserManager.RemoveClaimsAsync(user, claims);

                // remove attrs
                var attrs = await _dbContext.UserAttributes
                    .AsNoTracking()
                    .Where(_ => _.UserId == user.Id)
                    .ToListAsync();

                if (attrs != null && attrs.Any())
                {
                    _dbContext.UserAttributes.RemoveRange(attrs);
                    await _dbContext.SaveChangesAsync();
                }

                // delete user
                var result = await UserManager.DeleteAsync(user);

                action.Result = result.Succeeded;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute DeleteAsync method {@0} {UserName}",
                    id, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<bool>> IsEmailAvail(string email)
        {
            var action = new ActionResultDto<bool>();

            try
            {
                var found = await UserManager.FindByEmailAsync(email);
                action.Result = found != null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute IsEmailAvail method {@0} {UserName}",
                    email, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<UserDto>> GetByExternalIdAsync(string externalId)
        {
            var action = new ActionResultDto<UserDto>();

            try
            {
                var entity = await UserManager.Users
                        .Where(_ => _.ExternalId == externalId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                action.Result = entity == null ? null : Mapper.Map<UserDto>(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute GetByExternalIdAsync method {@0} {UserName}",
                    externalId, UserSession.UserName);
            }
            return action;
        }

        protected virtual async Task UpsertAttributesAsync(AppUser entity, UserDto dto)
        {
            if (dto.Attributes == null)
                dto.Attributes = new List<UserAttributeDto>();

            var attrs = await _dbContext.UserAttributes
                .AsNoTracking()
                .Where(_ => _.UserId == entity.Id)
                .ToListAsync();

            var claims = await UserManager.GetClaimsAsync(entity);
            if (claims == null) claims = new List<Claim>();

            var date = DateTime.Now;
            var username = UserSession.UserName;

            // update             
            foreach (var attr in attrs)
            {
                var found = dto.Attributes
                    .FirstOrDefault(_ => _.Type == attr.Type);
                if (found == null) continue;
                if (found.Value == attr.Value) continue;

                attr.Value = found.Value;
                attr.DateUpdated = date;
                attr.UpdatedBy = username;

                _dbContext.UserAttributes.Update(attr);
            }

            foreach (var claim in claims)
            {
                var found = dto.Attributes
                    .FirstOrDefault(_ => _.Type == claim.Type);
                if (found == null) continue;
                if (found.Value == claim.Value) continue;

                await UserManager.ReplaceClaimAsync(entity, claim,
                    new Claim(found.Type, found.Value));
            }

            // add
            foreach (var attr in dto.Attributes)
            {
                var found = attrs.FirstOrDefault(_ => _.Type == attr.Type);
                if (found != null) continue;

                _dbContext.UserAttributes.Add(new UserAttribute()
                {
                    Type = attr.Type,
                    Value = attr.Value,
                    UserId = entity.Id,
                    DateCreated = date,
                    DateUpdated = date,
                    UpdatedBy = username,
                    CreatedBy = username
                });
            }

            foreach (var attr in dto.Attributes)
            {
                var found = claims.FirstOrDefault(_ => _.Type == attr.Type);
                if (found != null) continue;

                await UserManager.AddClaimAsync(entity, 
                    new Claim(attr.Type, attr.Value));
            }

            // remove
            foreach (var attr in attrs)
            {
                var found = dto.Attributes.Any(_ => _.Type == attr.Type);
                if (found) continue;

                _dbContext.UserAttributes.Remove(attr);
            }

            foreach (var claim in claims)
            {
                var found = dto.Attributes.Any(_ => _.Type == claim.Type);
                if (found) continue;

                await UserManager.RemoveClaimAsync(entity, claim);
            }

            await _dbContext.SaveChangesAsync();
        }

        protected virtual async Task UpsertRolesAsync(AppUser entity, UserDto dto)
        {
            if (dto.Roles == null)
                dto.Roles = new List<string>();

            var roles = await UserManager.GetRolesAsync(entity);

            // add
            foreach (var role in dto.Roles)
            {
                var found = roles.FirstOrDefault(_ => _ == role);
                if (found != null) continue;

                await UserManager.AddToRoleAsync(entity, role);
            }

            // remove
            foreach (var role in roles)
            {
                var found = dto.Roles.Any(_ => _ == role);
                if (found) continue;

                await UserManager.RemoveFromRoleAsync(entity, role);
            }
        }
    }
}
