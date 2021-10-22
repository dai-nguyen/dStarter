using Infrastructure.Entities;

namespace Infrastructure.Modules.Bank.Entities
{
    public class BankAccountLink : BaseEntity
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}
