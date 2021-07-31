namespace Shared.DTOs.CRM
{
    public class TicketTableOptionDto : TableOptionDto
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public bool? IsBilled { get; set; }
        public bool? IsPaid { get; set; }
        public string CustomerId { get; set; }
        public string ContactId { get; set; }        
    }
}
