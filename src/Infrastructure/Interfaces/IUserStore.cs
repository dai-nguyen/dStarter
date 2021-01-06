using Infrastructure.Specifications;
using Infrastructure.Stores;
using Shared.DTOs;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserStore
    {
        Task<ActionResultDto<UserDto>> AddAsync(UserDto dto);
        Task<ActionResultDto<PageDto<UserDto>>> FindAsync(UserSpecification spec);
        Task<ActionResultDto<UserDto>> GetAsync(string id, UserKey key);
        Task<ActionResultDto<UserDto>> UpdateAsync(UserDto dto);
        Task<ActionResultDto<bool>> DeleteAsync(string id);
        Task<ActionResultDto<bool>> IsEmailAvail(string email);
        Task<ActionResultDto<UserDto>> GetByExternalIdAsync(string externalId);
        Task<ActionResultDto<string[]>> GetAllUserNamesAsync();
    }
}