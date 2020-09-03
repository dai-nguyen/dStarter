using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomAttributeDefinitionsController : Controller
    {
        private readonly ILogger<CustomAttributeDefinitionsController> _logger;
        private readonly IStore<CustomAttributeDefinition, CustomAttributeDefinitionDto> _store;

        public CustomAttributeDefinitionsController(
            ILogger<CustomAttributeDefinitionsController> logger,
            IStore<CustomAttributeDefinition, CustomAttributeDefinitionDto> store)
        {
            _logger = logger;
            _store = store;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<CustomAttributeDefinitionDto>>> LoadData([FromBody] TableOptionDto model)
        {
            var spec = model.ToCustomAttributeDefinitionSpecification();
            return await _store.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<CustomAttributeDefinitionDto>> GetData(int id)
        {
            return await _store.GetAsync(id);
        }

        [HttpPost]
        public async Task<ActionResultDto<CustomAttributeDefinitionDto>> Upsert([FromBody] CustomAttributeDefinitionDto dto)
        {
            if (dto.Id <= 0)
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

        [HttpGet]
        public ActionResultDto<string[]> ObjectNames()
        {
            var action = new ActionResultDto<string[]>();
            action.Result = Helper.GetParentNames();
            return action;
        }

        [HttpGet]
        public ActionResultDto<string[]> DataTypes()
        {
            var action = new ActionResultDto<string[]>();
            action.Result = Helper.GetDataTypes();
            return action;
        }
    }
}
