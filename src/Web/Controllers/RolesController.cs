using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Threading.Tasks;
using Web.Mappers;
using Web.Models;

namespace Web.Controllers
{
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
        public async Task<ActionResultDto<PageDto<RoleDto>>> LoadData([FromBody] TableOption model)
        {
            var spec = model.ToRoleSpecification();
            return await _roleStore.FindAsync(spec);
        }

        [HttpPost]
        public async Task<ActionResultDto<RoleDto>> GetData(string id)
        {
            return await _roleStore.GetAsync(id, Infrastructure.Stores.RoleKey.Id);
        }
    }
}
