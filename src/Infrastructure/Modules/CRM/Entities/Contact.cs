using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Modules.CRM.Entities
{
    public class Contact : BaseEntity
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        
    }
}
