using Infrastructure.Entities;

namespace Infrastructure.Modules.Bank.Entities
{
    public class BankAccountTransaction : BaseEntity
    {
        public string Name { get; set; }
        public double Amount { get; set; }
    }
}
