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

        [HttpPost]
        public async Task<ActionResultDto<PageDto<CustomerDto>>> LoadData(
            [FromBody] TableOptionDto model)
        {
            var spec = model.ToCustomerSpecification();
            return await _customerStore.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<CustomerDto>> GetData(int id)
        {
            return await _customerStore.GetAsync(id);
        }

        [HttpPost]
        public async Task<ActionResultDto<CustomerDto>> Upsert(
            [FromBody] CustomerDto dto)
        {
            if (dto.Id <= 0)
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
        public async Task<ActionResultDto<bool>> Delete(int id)
        {
            return await _customerStore.DeleteAsync(id);
        }
    }
}
