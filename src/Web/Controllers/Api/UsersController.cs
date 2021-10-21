using Infrastructure.Interfaces;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Server.AspNetCore;
using Shared.DTOs;
using System.Threading.Tasks;

namespace Web.Controllers.Api
{
    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserStore _userStore;

        public UsersController(
            ILogger<UsersController> logger,
            IUserStore userStore)
        {
            _logger = logger;
            _userStore = userStore;
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<UserDto>>> LoadData([FromBody] TableOptionDto model)
        {
            var spec = model.ToUserSpecification();
            return await _userStore.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<UserDto>> GetData(string id)
        {
            return await _userStore.GetAsync(id, Infrastructure.Stores.UserKey.Id);
        }

        [HttpPost]
        public async Task<ActionResultDto<UserDto>> UpsertData([FromBody] UserDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                // create
                return await _userStore.AddAsync(dto);
            }
            else
            {
                // update
                return await _userStore.UpdateAsync(dto);
            }
        }

        [HttpDelete]
        public async Task<ActionResultDto<bool>> Delete(string id)
        {
            return await _userStore.DeleteAsync(id);
        }
    }
}
