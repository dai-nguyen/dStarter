using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual DateTime DateUpdated { get; set; } = DateTime.Now;
        public virtual string CreatedBy { get; set; } = "?";
        public virtual string UpdatedBy { get; set; } = "?";
        public virtual string ExternalId { get; set; }

        public ICollection<CustomAttribute> CustomAttributes { get; set; }
    }
}
