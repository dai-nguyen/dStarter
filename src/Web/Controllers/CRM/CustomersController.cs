using Infrastructure.Modules.CRM.Entities;
using Infrastructure.Modules.CRM.Mappers;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.DTOs.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.CRM
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly GenericService<Customer, CustomerDto, CustomerDto> _service;

        public CustomersController(
            ILogger<CustomersController> logger,
            GenericService<Customer, CustomerDto, CustomerDto> service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResultDto<PageDto<CustomerDto>>> LoadData([FromBody] TableOptionDto model)
        {
            var spec = model.ToCustomerSpecification();
            return await _service.FindAsync(spec);
        }
    }
}
