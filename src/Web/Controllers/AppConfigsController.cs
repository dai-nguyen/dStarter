using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class AppConfigsController : Controller
    {
        private readonly ILogger<LogsController> _logger;
        private readonly IUserSession _userSession;
        private readonly IStore<AppConfig, AppConfigDto> _store;

        public AppConfigsController(
            ILogger<LogsController> logger,
            IUserSession userSession,
            IStore<AppConfig, AppConfigDto> store)
        {
            _logger = logger;
            _userSession = userSession;
            _store = store;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<AppConfigDto>>> LoadData(
            [FromBody] AppConfigTableOptionDto model)
        {
            var spec = model.ToAppConfigSpecification();
            return await _store.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<AppConfigDto>> GetData(string id)
        {
            return await _store.GetAsync(id);
        }

        [HttpPost]
        public async Task<ActionResultDto<AppConfigDto>> Upsert(
            [FromBody] AppConfigDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                // create
                return await _store.AddAsync(dto);
            }
            else
            {
                // update
                return await _store.UpdateAsync(dto);
            }
        }

        [HttpDelete]
        public async Task<ActionResultDto<bool>> Delete(string id)
        {
            return await _store.DeleteAsync(id);
        }
    }
}
