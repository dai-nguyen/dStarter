using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Web.Mappers;
using Web.Models;

namespace Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IUserStore _userStore;

        public RolesController(
            ILogger<RolesController> logger,
            IUserStore userStore)
        {
            _logger = logger;
            _userStore = userStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<UserDto>>> LoadData([FromBody] TableOption model)
        {
            var spec = model.ToUserSpecification();
            return await _userStore.FindAsync(spec);
        }

        [HttpPost]
        public async Task<ActionResultDto<UserDto>> GetData(string id)
        {
            return await _userStore.GetAsync(id, Infrastructure.Stores.UserKey.Id);
        }
    }
}
