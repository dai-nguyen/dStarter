using Infrastructure.Interfaces;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Threading.Tasks;

namespace Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRoleStore _roleStore;

        public RolesController(
            ILogger<RolesController> logger,
            IRoleStore roleStore)
        {
            _logger = logger;
            _roleStore = roleStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<RoleDto>>> LoadData(
            [FromBody] TableOptionDto model)
        {
            var spec = model.ToRoleSpecification();
            return await _roleStore.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<RoleDto>> GetData(string id)
        {
            return await _roleStore.GetAsync(id, Infrastructure.Stores.RoleKey.Id);
        }

        [HttpPost]
        public async Task<ActionResultDto<RoleDto>> Upsert(
            [FromBody] RoleDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                // create
                return await _roleStore.AddAsync(dto);
            }
            else
            {
                // update
                return await _roleStore.UpdateAsync(dto);
            }
        }

        [HttpDelete]
        public async Task<ActionResultDto<bool>> Delete(string id)
        {
            return await _roleStore.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<ActionResultDto<RoleDto[]>> Roles()
        {
            return await _roleStore.ListAllAsync();
        }
    }
}
