using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Modules.CRM.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

        public virtual IEnumerable<Contact> Contacts { get; set; }        
    }
}
