using Infrastructure.Interfaces;
using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Modules.CRM.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.DTOs.CRM;
using System;
using System.Threading.Tasks;

namespace Web.Controllers.CRM
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ILogger<TicketsController> _logger;
        private readonly IStore<Ticket, TicketDto> _ticketStore;
        
        public TicketsController(
            ILogger<TicketsController> logger,
            IStore<Ticket, TicketDto> ticketStore
            )
        {
            _logger = logger;
            _ticketStore = ticketStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(
            string id, 
            string customer_id, 
            string contact_id)
        {
            ViewBag.id = id ?? "";
            ViewBag.customer_id = customer_id ?? "";
            ViewBag.contact_id = contact_id ?? "";
            
            return View("Upsert");
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<TicketDto>>> LoadData(
            [FromBody] TicketTableOptionDto model)
        {
            var spec = model.ToTicketSpecification();
            return await _ticketStore.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<TicketDto>> GetData(string id)
        {
            return await _ticketStore.GetAsync(id);
        }

        [HttpGet]
        public string[] GetStatusData()
        {
            //return Enum.GetNames(typeof(TicketStatus));
            return Infrastructure.Modules.CRM.Constants.TicketStatuses();
        }

        [HttpPost]
        public async Task<ActionResultDto<TicketDto>> UpsertData(
            [FromBody] TicketDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                // create
                return await _ticketStore.AddAsync(dto);
            }
            else
            {
                // update
                return await _ticketStore.UpdateAsync(dto);
            }
        }

        [HttpDelete]
        public async Task<ActionResultDto<bool>> DeleteData(string id)
        {
            return await _ticketStore.DeleteAsync(id);
        }


    }
}
