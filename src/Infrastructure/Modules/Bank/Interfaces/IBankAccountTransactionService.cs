using Shared.DTOs;
using Shared.DTOs.Bank;
using System.Threading.Tasks;

namespace Infrastructure.Modules.Bank.Interfaces
{
    public interface IBankAccountTransactionService
    {
        Task<ActionResultDto<BankAccountTransactionDto>> AddTransactionAsync(BankAccountTransactionDto dto);
    }
}
