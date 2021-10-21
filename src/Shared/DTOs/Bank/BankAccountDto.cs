namespace Shared.DTOs.Bank
{
    public class BankAccountDto : BaseDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public double Amount { get; set; }
    }
}
