using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Linq;

namespace Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IUserSession _userSession;

        public ProfileController(
            ILogger<ProfileController> logger,
            IUserSession userSession)
        {
            _logger = logger;
            _userSession = userSession;
        }

        [HttpGet]
        public ActionResultDto<UserProfileDto> GetProfile()
        {
            var profile = new UserProfileDto()
            {
                UserId = _userSession.UserId,
                UserName = _userSession.UserName,
                Roles = _userSession.Roles.ToArray()
            };

            return new ActionResultDto<UserProfileDto>()
            {
                Result = profile
            };
        }
    }
}
