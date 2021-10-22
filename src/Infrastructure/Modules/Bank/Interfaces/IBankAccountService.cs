using Shared.DTOs;
using Shared.DTOs.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Modules.Bank.Interfaces
{
    public interface IBankAccountService
    {
        Task<ActionResultDto<BankAccountTransactionDto>> AddTransactionAsync(BankAccountTransactionDto dto);
    }
}
