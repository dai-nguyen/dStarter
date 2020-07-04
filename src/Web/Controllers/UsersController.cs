﻿using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Threading.Tasks;
using Web.Mappers;
using Web.Models;

namespace Web.Controllers
{
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

        [HttpGet]
        public async Task<ActionResultDto<UserDto>> GetData(string id)
        {
            return await _userStore.GetAsync(id, Infrastructure.Stores.UserKey.Id);
        }

        [HttpPost]
        public async Task<ActionResultDto<UserDto>> Upsert([FromBody] UserDto dto)
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