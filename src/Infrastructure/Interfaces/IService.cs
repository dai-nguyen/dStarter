using Infrastructure.Entities;
using Shared.DTOs;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IService<TEntity, TaDto, TDto>
        where TEntity : BaseEntity
        where TaDto : BaseWithAttrDto
        where TDto: BaseDto
    {

        Task<ActionResultDto<TaDto>> GetAsync(int id);
        Task<ActionResultDto<TaDto>> AddAsync(TaDto adto);

    }
}
