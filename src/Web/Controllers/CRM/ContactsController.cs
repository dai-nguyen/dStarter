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
    public class ContactsController : Controller
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IStore<Contact, ContactDto> _contactStore;
        
        public ContactsController(
            ILogger<ContactsController> logger,
            IStore<Contact, ContactDto> contactStore
            )
        {
            _logger = logger;
            _contactStore = contactStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(string id, string customer_id)
        {
            ViewBag.id = id ?? "";
            ViewBag.customer_id = customer_id ?? "";

            return View("Upsert");
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<ContactDto>>> LoadData(
            [FromBody] ContactTableOptionDto model)
        {
            var spec = model.ToContactSpecification();
            return await _contactStore.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<ContactDto>> GetData(string id)
        {
            return await _contactStore.GetAsync(id);
        }

        [HttpPost]
        public async Task<ActionResultDto<ContactDto>> UpsertData(
            [FromBody] ContactDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                // create
                return await _contactStore.AddAsync(dto);
            }
            else
            {
                // update
                return await _contactStore.UpdateAsync(dto);
            }
        }

        [HttpDelete]
        public async Task<ActionResultDto<bool>> DeleteData(string id)
        {
            return await _contactStore.DeleteAsync(id);
        }
    }
}
