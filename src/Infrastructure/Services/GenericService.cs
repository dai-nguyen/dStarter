using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class GenericService<TEntity, TaDto, TDto> : IService<TEntity, TaDto, TDto>
        where TEntity : BaseEntity
        where TaDto : BaseWithAttrDto
        where TDto: BaseDto
    {
        protected readonly ILogger Logger;
        protected readonly IStore<TEntity, TDto> Store;
        protected readonly IStore<CustomAttribute, CustomAttributeDto> CustAttrStore;
        protected readonly IStore<CustomAttributeDefinition, CustomAttributeDefinitionDto> CustAttrDefStore;
        protected readonly IMapper Mapper;
        protected readonly IUserSession UserSession;

        public GenericService(
            ILogger<GenericService<TEntity, TaDto, TDto>> logger,
            IStore<CustomAttribute, CustomAttributeDto> custAttrStore,
            IStore<CustomAttributeDefinition, CustomAttributeDefinitionDto> custAttrDefStore,
            IStore<TEntity, TDto> store,
            IMapper mapper, 
            IUserSession userSession)
        {
            Logger = logger;
            Store = store;
            CustAttrStore = custAttrStore;
            CustAttrDefStore = custAttrDefStore;
            Mapper = mapper;
            UserSession = userSession;
        }

        public virtual async Task<ActionResultDto<TaDto>> GetAsync(int id)
        {
            var action = new ActionResultDto<TaDto>();
            try
            {
                var data = await Store.GetAsync(id);

                if (data == null) return action;

                action.Result = Mapper.Map<TaDto>(data.Result);

                var type = typeof(IEntity);
                var typeName = type.Name;

                var defSpec = new CustomAttributeDefinitionSpecification(
                    "",
                    typeName, 
                    new BaseFilterDto(null, "DisplayName", "asc", 0, 0, false),
                    CustomAttributeDefinitionMapper.CustAttrDefColMaps);

                var defAction = await CustAttrDefStore.FindAsync(defSpec);

                if (defAction.Result == null || defAction.Result.Total == 0)
                    return action;

                var defIds = defAction.Result.Data.Select(_ => _.Id).ToArray();

                var attrSpec = new CustomAttributeSpecification(
                    defIds,
                    id,
                    new BaseFilterDto(null, "DisplayName", "asc", 0, 0, false),
                    CustomAttributeMapper.CustAttrColMaps
                    );

                attrSpec.Includes.Add(_ => _.Definition);

                var attrAction = await CustAttrStore.FindAsync(attrSpec);

                var attrs = new List<CustomAttributeDto>();

                if (attrAction.Result != null && defAction.Result.Total > 0)
                {
                    attrs.AddRange(Mapper.Map<CustomAttributeDto[]>(attrAction.Result.Data));
                }

                int count = 0;
                foreach (var def in defAction.Result.Data)
                {
                    var found = attrs.Any(_ => _.DefinitionId == def.Id);

                    if (found) continue;

                    count -= 1;
                    attrs.Add(new CustomAttributeDto()
                    {
                        DefinitionId = def.Id,
                        Value = "",
                        Id = count,
                        Definition = Mapper.Map<CustomAttributeDefinitionDto>(def)
                    });
                }

                action.Result.CustomAttributes = attrs.ToArray();

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute GetAsync method {0} {UserName}",
                    id, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<TaDto>> AddAsync(TaDto adto)
        {
            var action = new ActionResultDto<TaDto>();

            if (adto == null) return action;

            try
            {
                var dto = Mapper.Map<TDto>(adto);

                if (dto == null)
                {
                    var err = "Unable to map to Dto";
                    Logger.LogError(err);
                    action.Errors.Add(err);
                    return action;
                }

                // add main object
                await Store.AddAsync(dto);

                // handle custom attribute
                if (adto.CustomAttributes != null 
                    && adto.CustomAttributes.Length > 0)
                {
                    foreach (var attr in adto.CustomAttributes)
                    {
                        if (string.IsNullOrEmpty(attr.Value))
                            continue;

                        await CustAttrStore.AddAsync(attr);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute AddAsync method {@0} {UserName}",
                    adto, UserSession.UserName);
            }
            return action;
        }

        public virtual async Task<ActionResultDto<TaDto>> UpdateAsync(TaDto adto)
        {
            var action = new ActionResultDto<TaDto>();

            if (adto == null) return action;

            try
            {
                var dto = Mapper.Map<TDto>(adto);

                await Store.UpdateAsync(dto);

                await UpsertCustomAttributesAsync(adto);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to execute UpdateAsync method {@0} {UserName}",
                    adto, UserSession.UserName);
            }

            return action;
        }

        public virtual async Task<ActionResultDto<PageDto<TDto>>> FindAsync(ISpecification<TEntity> spec)
        {
            return await Store.FindAsync(spec);
        }

        protected virtual async Task UpsertCustomAttributesAsync(TaDto adto)
        {
            if (adto == null) return;

            var getAction = await this.GetAsync(adto.Id);

            if (getAction.Result == null
                || getAction.Result.CustomAttributes == null
                || getAction.Result.CustomAttributes.Length == 0) return;

            // update
            foreach (var attr in adto.CustomAttributes)
            {
                if (attr.Id <= 0) continue;

                if (string.IsNullOrEmpty(attr.Value))
                    await CustAttrStore.DeleteAsync(attr.Id);
                else
                    await CustAttrStore.UpdateAsync(attr);
            }

            // add
            foreach (var attr in adto.CustomAttributes)
            {
                if (attr.Id > 0) continue;
                if (string.IsNullOrEmpty(attr.Value)) continue;

                await CustAttrStore.AddAsync(attr);
            }
        }
    }
}
