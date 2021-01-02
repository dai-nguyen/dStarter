using Infrastructure.Specifications;
using Shared.DTOs;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ILogMsgStore
    {
        Task<int> DeleteAsync(DateTime date);
        Task<ActionResultDto<PageDto<LogMsgDto>>> FindAsync(LogMsgSpecification spec);
    }
}
