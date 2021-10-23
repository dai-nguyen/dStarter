namespace Shared.DTOs.Bank
{
    public class BankAccountTransactionDto : BaseDto
    {
        public string Name { get; set; }
        public double Amount { get; set; }        
        public string UserId { get; set; }
        public string BankAccountId { get; set; }
    }
}
