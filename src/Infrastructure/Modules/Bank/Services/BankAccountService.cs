using Infrastructure.Interfaces;
using Infrastructure.Modules.Bank.Entities;
using Infrastructure.Modules.Bank.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.DTOs.Bank;
using System.Threading.Tasks;

namespace Infrastructure.Modules.Bank.Services
{
    public class BankAccountService : IBankAccountService
    {
        readonly ILogger _logger;
        readonly IStore<BankAccount, BankAccountDto> _bankAccountStore;
        readonly IStore<BankAccountTransaction, BankAccountTransactionDto> _bankAccountTransactionStore;

        public BankAccountService(
            ILogger<BankAccountService> logger,
            IStore<BankAccount, BankAccountDto> bankAccountStore,
            IStore<BankAccountTransaction, BankAccountTransactionDto> bankAccountTransactionStore)
        {
            _logger = logger;
            _bankAccountStore = bankAccountStore;
            _bankAccountTransactionStore = bankAccountTransactionStore;
        }

        public virtual async Task<ActionResultDto<BankAccountTransactionDto>> AddTransactionAsync(BankAccountTransactionDto dto)
        {
            var action = new ActionResultDto<BankAccountTransactionDto>();

            var bankGetAction = await _bankAccountStore.GetAsync(dto.BankAccountId);

            var bank = bankGetAction.Result;

            if (bank == null)
            {
                _logger.LogError("Unable to get BankAccount ID {0}", dto.BankAccountId);
                return action;
            }

            var transactionAddAction = await _bankAccountTransactionStore.AddAsync(dto);

            if (transactionAddAction.Result == null)
            {
                _logger.LogError("Unable to create BankAccountTransaction {@0}", dto);
                return action;
            }

            bank.Amount += dto.Amount;

            var bankUpdateAction = await _bankAccountStore.UpdateAsync(bank);

            if (bankUpdateAction.Result == null)
            {
                // rollback
                _logger.LogError("Unable to update BankAccount {@0}", bank);
                _logger.LogError("Rollback / remove Transaction ID {0}", transactionAddAction.Result.Id);
                await _bankAccountTransactionStore.DeleteAsync(transactionAddAction.Result.Id);
                return action;
            }

            return transactionAddAction;
        }
    }
}
