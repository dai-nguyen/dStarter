using Infrastructure.Specifications;
using Infrastructure.Stores;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IRoleStore
    {
        Task<ActionResultDto<RoleDto>> AddAsync(RoleDto dto);
        Task<ActionResultDto<PageDto<RoleDto>>> FindAsync(RoleSpecification spec);
        Task<ActionResultDto<RoleDto>> GetAsync(string id, RoleKey key);
        Task<ActionResultDto<RoleDto>> UpdateAsync(RoleDto dto);
        Task<ActionResultDto<bool>> DeleteAsync(string id);
        Task<ActionResultDto<RoleDto>> GetByExternalIdAsync(string externalId);
        Task<ActionResultDto<RoleDto[]>> ListAllAsync();

    }
}
