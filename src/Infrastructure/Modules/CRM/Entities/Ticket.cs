using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Modules.CRM.Entities
{
    public class Ticket : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool IsBilled { get; set; }
        public bool IsPaid { get; set; }

        public int? ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual IEnumerable<LaborHour> LaborHours { get; set; }
    }
}
