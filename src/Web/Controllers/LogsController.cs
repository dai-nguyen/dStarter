using Infrastructure.Interfaces;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class LogsController : Controller
    {
        private readonly ILogger<LogsController> _logger;
        private readonly ILogMsgStore _logMsgStore;
        private readonly IUserSession _userSession;
        private readonly IUserStore _userStore;

        public LogsController(
            ILogger<LogsController> logger,
            ILogMsgStore logMsgStore,
            IUserSession userSession,
            IUserStore userStore)
        {
            _logger = logger;
            _logMsgStore = logMsgStore;
            _userSession = userSession;
            _userStore = userStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<LogMsgDto>>> LoadData(
            [FromBody] LogMsgTableOptionDto model)
        {
            var spec = model.ToLogMsgSpecification();
            return await _logMsgStore.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<string[]>> GetUsers()
        {
            var dto = new ActionResultDto<string[]>();

            var users = new List<string>();

            if (_userSession.Roles.Contains("Admin"))
            {
                var action = await _userStore.GetAllUserNamesAsync();
                users.AddRange(action.Result);
                users.Add("Unknown");
            }
            else
            {
                users.Add(_userSession.UserName);
            }

            dto.Result = users.ToArray();

            return dto;
        }
    }
}
