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
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IStore<Customer, CustomerDto> _customerStore;
        
        public CustomersController(
            ILogger<CustomersController> logger,
            IStore<Customer, CustomerDto> customerStore
            )
        {
            _logger = logger;
            _customerStore = customerStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(string id)
        {
            return View("Upsert", id);
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<CustomerDto>>> LoadData(
            [FromBody] TableOptionDto model)
        {
            var spec = model.ToCustomerSpecification();
            return await _customerStore.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<CustomerDto>> GetData(string id)
        {
            return await _customerStore.GetAsync(id);
        }

        [HttpPost]
        public async Task<ActionResultDto<CustomerDto>> UpsertData(
            [FromBody] CustomerDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                // create
                return await _customerStore.AddAsync(dto);
            }
            else
            {
                // update
                return await _customerStore.UpdateAsync(dto);
            }
        }

        [HttpDelete]
        public async Task<ActionResultDto<bool>> DeleteData(string id)
        {
            return await _customerStore.DeleteAsync(id);
        }
    }
}
