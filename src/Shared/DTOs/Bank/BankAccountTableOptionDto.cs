namespace Shared.DTOs.Bank
{
    public class BankAccountTransactionTableOptionDto : TableOptionDto
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public bool IsRealized { get; set; }
        public string UserId { get; set; }
        public string BankAccountId { get; set; }
    }
}
