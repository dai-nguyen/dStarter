using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Modules.Bank.Entities
{
    public class BankAccount : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }

        public double Amount { get; set; }

        public IEnumerable<BankAccountTransaction> Transactions { get; set; }
    }
}
