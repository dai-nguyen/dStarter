using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Stores
{
    public class GenericStore<TEntity, TDto> : IStore<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : BaseDto
    {
        protected readonly ILogger Logger;
        protected readonly AppDbContext DbContext;
        protected IMapper Mapper;
        protected IUserSession UserSession;

        public GenericStore(
            ILogger<GenericStore<TEntity, TDto>> logger,
            AppDbContext dbContext,
            IMapper mapper,
            IUserSession userSession
            )
        {
            Logger = logger;
            DbContext = dbContext;
            Mapper = mapper;
            UserSession = userSession;
        }

        public virtual async Task<ActionResultDto<TDto>> AddAsync(TDto dto)
        {
            var action = new ActionResultDto<TDto>();

            if (dto == null)
                return action;

            try
            {
                var entity = Mapper.Map<TEntity>(dto);

                if (entity == null)
                {
                    var err = "Unable to map to entity";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                if (string.IsNullOrEmpty(entity.Id))
                    entity.Id = Guid.NewGuid().ToString();

                Logger.LogInformation("Adding Dto {@0} Entity {@1} {UserName}",
                    dto, entity, UserSession.UserName);

                await DbContext.Set<TEntity>().AddAsync(entity);
                int count = await DbContext.SaveChangesAsync();

                action.Result = Mapper.Map<TDto>(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute AddAsync method {@0} {UserName}",
                    dto, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<TDto[]>> AddRangeAsync(TDto[] dtos)
        {
            var action = new ActionResultDto<TDto[]>();

            if (dtos == null || !dtos.Any())
                return action;

            try
            {
                var entities = Mapper.Map<TEntity[]>(dtos);

                if (entities == null || !entities.Any())
                {
                    var err = "Unable to map to entities";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                foreach (var entity in entities)
                {
                    if (string.IsNullOrEmpty(entity.Id))
                        entity.Id = Guid.NewGuid().ToString();
                }

                Logger.LogInformation("Adding DTOs {@0} Entities {@1} {UserName}",
                    dtos, entities, UserSession.UserName);

                await DbContext.Set<TEntity>().AddRangeAsync(entities);
                await DbContext.SaveChangesAsync();
                action.Result = Mapper.Map<TDto[]>(entities);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute AddRangeAsync method {@0} {UserName}",
                    dtos, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<bool>> DeleteAsync(string id)
        {
            var action = new ActionResultDto<bool>() { Result = false };

            try
            {
                var entity = await DbContext.Set<TEntity>()
                    .FindAsync(id);

                if (entity == null)
                {
                    Logger.LogError($"Entity with Id {id} not found.");
                    return action;
                }

                Logger.LogInformation("Removimg entity {@0} {UserName}",
                    entity, UserSession.UserName);

                DbContext.Set<TEntity>().Remove(entity);

                var count = await DbContext.SaveChangesAsync();
                action.Result = count > 0;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute DeleteAsync {0}",
                    id, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<PageDto<TDto>>> FindAsync(ISpecification<TEntity> spec)
        {
            var action = new ActionResultDto<PageDto<TDto>>();

            if (spec == null)
                return action;

            try
            {
                var query = ApplySpecification(spec);
                var total = await query.LongCountAsync();
                var data = await SpecificationEvaluator<TEntity>
                    .ApplyPaging(query, spec)
                    .ToListAsync();

                action.Result = new PageDto<TDto>(total, Mapper.Map<TDto[]>(data));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute FindAsync method {@0} {UserName}",
                    spec, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<TDto>> GetAsync(string id)
        {
            var action = new ActionResultDto<TDto>();
            try
            {
                var data = await DbContext.Set<TEntity>()
                    .FindAsync(id);

                action.Result = data == null ? null : Mapper.Map<TDto>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute GetAsync method {0} {UserName}",
                    id, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<TDto>> UpdateAsync(TDto dto)
        {
            var action = new ActionResultDto<TDto>();

            if (dto == null)
                return action;

            try
            {
                var entity = await DbContext.Set<TEntity>()
                    .FindAsync(dto.Id);

                if (entity == null)
                {
                    var err = $"Unable to find entity with Id {dto.Id}";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                entity = Mapper.Map(dto, entity);

                if (entity == null)
                {
                    var err = $"Unable to map to entity with Id {dto.Id}";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                Logger.LogInformation("Updating Dto {@0} Entity {@1} {UserName}",
                    dto, entity, UserSession.UserName);

                DbContext.Set<TEntity>().Update(entity);
                //DbContext.Entry(entity).State = EntityState.Modified;

                await DbContext.SaveChangesAsync();
                action.Result = Mapper.Map<TDto>(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute UpdateAsync method {@0}",
                    dto, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<TDto[]>> UpdateRangeAsync(TDto[] dtos)
        {
            var action = new ActionResultDto<TDto[]>();

            if (dtos == null || !dtos.Any())
                return action;

            try
            {
                var ids = dtos.Select(_ => _.Id).ToArray();

                var entities = await DbContext.Set<TEntity>()
                    .Where(_ => ids.Contains(_.Id))
                    .ToListAsync();

                if (entities == null)
                {
                    var err = $"Unable to find entities with IDs {string.Join(",", ids)}";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                entities = Mapper.Map(dtos, entities);

                if (entities == null || !entities.Any())
                {
                    var err = $"Unable to map to entities with IDs {string.Join(",", ids)}";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                Logger.LogInformation("Updating DTOs {@0} Entities {@1} {UserName}",
                    dtos, entities, UserSession.UserName);

                DbContext.Set<TEntity>().UpdateRange(entities);

                await DbContext.SaveChangesAsync();
                action.Result = Mapper.Map<TDto[]>(entities);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute UpdateRangeAsync {@0}",
                    dtos, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<TDto>> GetByExternalIdAsync(string externalId)
        {
            var action = new ActionResultDto<TDto>();

            if (string.IsNullOrEmpty(externalId) || string.IsNullOrWhiteSpace(externalId))
                return action;

            try
            {
                var data = await DbContext.Set<TEntity>()
                    .FirstOrDefaultAsync(_ => _.ExternalId == externalId);

                action.Result = data == null ? null : Mapper.Map<TDto>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute GetByExternalIdAsync {0} {UserName}",
                    externalId, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<TDto>> PatchAsync(
            string id,
            JsonPatchDocument<TDto> patchDoc)
        {
            var action = new ActionResultDto<TDto>();

            if (string.IsNullOrEmpty(id) || patchDoc == null)
                return action;

            try
            {
                var getAction = await this.GetAsync(id);

                if (getAction.Result == null)
                {
                    var err = $"Unable to find entity with Id {id}";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                var dto = getAction.Result;

                patchDoc.ApplyTo(dto);

                action = await this.UpdateAsync(dto);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute PatchAsync method {@0} {UserName}",
                    patchDoc, UserSession.UserName);
            }
            return action;
        }


        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            var query = DbContext.Set<TEntity>().AsQueryable();
            return SpecificationEvaluator<TEntity>.GetQuery(query, spec);
        }
    }
}
