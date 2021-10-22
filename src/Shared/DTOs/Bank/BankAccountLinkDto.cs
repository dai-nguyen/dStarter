namespace Shared.DTOs.Bank
{
    public class BankAccountLinkDto : BaseDto
    {
        public string Name { get; set; }        
        public string UserId { get; set; }
        public string BankAccountId { get; set; }
    }
}
