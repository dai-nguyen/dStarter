using Infrastructure.Interfaces;
using Infrastructure.Modules.Bank.Entities;
using Infrastructure.Modules.Bank.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.DTOs.Bank;
using System.Threading.Tasks;

namespace Web.Controllers.Bank
{
    [Authorize]
    public class BankAccountsController : Controller
    {
        readonly ILogger _logger;
        readonly IStore<BankAccount, BankAccountDto> _bankAccountStore;
        readonly IUserSession _userSession;

        public BankAccountsController(
            ILogger<BankAccountsController> logger,
            IUserSession userSession,
            IStore<BankAccount, BankAccountDto> bankAccountStore
            )
        {
            _logger = logger;
            _userSession = userSession;
            _bankAccountStore = bankAccountStore;
            
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
        public async Task<ActionResultDto<PageDto<BankAccountDto>>> LoadData(
            [FromBody] BankAccountTableOptionDto model)
        {
            model.UserId = _userSession.UserId;
            var spec = model.ToBankAccountSpecification();
            
            return await _bankAccountStore.FindAsync(spec);
        }

        [HttpGet]
        public async Task<ActionResultDto<BankAccountDto>> GetData(string id)
        {            
            var data = await _bankAccountStore.GetAsync(id);

            if (data?.Result?.UserId != _userSession.UserId)
            {
                _logger.LogInformation($"UserId {_userSession.UserId} " +
                    $"is trying to access User {data?.Result?.UserId} data");
                return null;
            }

            return data;
        }

        [HttpPost]
        public async Task<ActionResultDto<BankAccountDto>> UpsertData(
            [FromBody] BankAccountDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                // create
                return await _bankAccountStore.AddAsync(dto);
            }
            else
            {
                // update
                return await _bankAccountStore.UpdateAsync(dto);
            }
        }

        [HttpPatch]
        public async Task<ActionResultDto<BankAccountDto>> PatchData(
            string id, 
            [FromBody] JsonPatchDocument<BankAccountDto> patchDoc)
        {
            return await _bankAccountStore.PatchAsync(id, patchDoc);
        }
    }
}
