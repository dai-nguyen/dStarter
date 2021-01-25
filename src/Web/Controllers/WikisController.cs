﻿using Infrastructure.Entities;
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
    public class WikisController : Controller
    {
        private readonly ILogger<WikisController> _logger;
        private readonly IUserSession _userSession;
        private readonly IStore<Wiki, WikiDto> _store;

        public WikisController(
            ILogger<WikisController> logger,
            IUserSession userSession,
            IStore<Wiki, WikiDto> store)
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
        public async Task<ActionResultDto<PageDto<WikiDto>>> LoadData(
            [FromBody] WikiTableOptionDto model)
        {
            var spec = model.ToWikiSpecification();
            return await _store.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<WikiDto>> GetData(int id)
        {
            return await _store.GetAsync(id);
        }

        [HttpPost]
        public async Task<ActionResultDto<WikiDto>> Upsert(
            [FromBody] WikiDto dto)
        {
            if (dto.Id < 1)
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
        public async Task<ActionResultDto<bool>> Delete(int id)
        {
            return await _store.DeleteAsync(id);
        }
    }
}
