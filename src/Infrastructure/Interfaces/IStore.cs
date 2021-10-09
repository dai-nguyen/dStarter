using Infrastructure.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Shared.DTOs;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStore<TEntity, TDto> 
        where TEntity : BaseEntity
        where TDto : BaseDto
    {
        Task<ActionResultDto<TDto>> AddAsync(TDto dto);
        Task<ActionResultDto<TDto[]>> AddRangeAsync(TDto[] dtos);
        Task<ActionResultDto<bool>> DeleteAsync(string id);
        Task<ActionResultDto<PageDto<TDto>>> FindAsync(ISpecification<TEntity> spec);
        Task<ActionResultDto<TDto>> GetAsync(string id);
        Task<ActionResultDto<TDto>> UpdateAsync(TDto dto);
        Task<ActionResultDto<TDto[]>> UpdateRangeAsync(TDto[] dtos);
        Task<ActionResultDto<TDto>> GetByExternalIdAsync(string externalId);
        Task<ActionResultDto<TDto>> PatchAsync(
            string id,
            JsonPatchDocument<TDto> patchDoc);        
    }
}
