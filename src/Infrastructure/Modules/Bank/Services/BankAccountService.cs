using Infrastructure.Interfaces;
using Infrastructure.Modules.Bank.Entities;
using Infrastructure.Modules.Bank.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.DTOs.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}
