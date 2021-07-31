namespace Shared.DTOs.CRM
{
    public class TicketDto : BaseWithAttrDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool IsBilled { get; set; }
        public bool IsPaid { get; set; }

        public string ContactId { get; set; }
    }
}
