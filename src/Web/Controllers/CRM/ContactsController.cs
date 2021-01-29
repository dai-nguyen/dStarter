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

        [HttpPost]
        public async Task<ActionResultDto<PageDto<ContactDto>>> LoadData(
            [FromBody] TableOptionDto model)
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
        public async Task<ActionResultDto<ContactDto>> Upsert(
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
        public async Task<ActionResultDto<bool>> Delete(string id)
        {
            return await _contactStore.DeleteAsync(id);
        }
    }
}
