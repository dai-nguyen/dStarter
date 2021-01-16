using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class AppConfigsController : Controller
    {
        private readonly ILogger<LogsController> _logger;
        private readonly IUserSession _userSession;

        public AppConfigsController(
            ILogger<LogsController> logger,
            IUserSession userSession)
        {
            _logger = logger;
            _userSession = userSession;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
