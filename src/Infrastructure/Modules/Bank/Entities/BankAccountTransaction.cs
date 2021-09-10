using Infrastructure.Entities;

namespace Infrastructure.Modules.Bank.Entities
{
    public class BankAccountTransaction : BaseEntity
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public bool IsRealized { get; set; }
        public string UserId { get; set; }
        public string BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}
