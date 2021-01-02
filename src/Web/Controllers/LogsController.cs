using Infrastructure.Interfaces;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class LogsController : Controller
    {
        private readonly ILogger<LogsController> _logger;
        private readonly ILogMsgStore _logMsgStore;

        public LogsController(
            ILogger<LogsController> logger,
            ILogMsgStore logMsgStore)
        {
            _logger = logger;
            _logMsgStore = logMsgStore;
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
    }
}
