using Infrastructure.Interfaces;
using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Modules.CRM.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.DTOs.CRM;
using System.Threading.Tasks;

namespace Web.Controllers.CRM
{
    [Authorize]
    public class LaborsController : Controller
    {
        private readonly ILogger<LaborsController> _logger;
        private readonly IStore<Labor, LaborDto> _laborStore;
        
        public LaborsController(
            ILogger<LaborsController> logger,
            IStore<Labor, LaborDto> laborStore
            )
        {
            _logger = logger;
            _laborStore = laborStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(
            string id, 
            string ticket_id)
        {
            ViewBag.id = id ?? "";
            ViewBag.ticket_id = ticket_id ?? "";            
            
            return View("Upsert");
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<LaborDto>>> LoadData(
            [FromBody] LaborTableOptionDto model)
        {
            var spec = model.ToLaborSpecification();
            return await _laborStore.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<LaborDto>> GetData(string id)
        {
            return await _laborStore.GetAsync(id);
        }
        
        [HttpPost]
        public async Task<ActionResultDto<LaborDto>> UpsertData(
            [FromBody] LaborDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                // create
                return await _laborStore.AddAsync(dto);
            }
            else
            {
                // update
                return await _laborStore.UpdateAsync(dto);
            }
        }

        [HttpDelete]
        public async Task<ActionResultDto<bool>> DeleteData(string id)
        {
            return await _laborStore.DeleteAsync(id);
        }


    }
}
