using Infrastructure.Entities;

namespace Infrastructure.Modules.CRM.Entities
{
    public class Labor : BaseEntity
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public string Description { get; set; }

        public string TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
