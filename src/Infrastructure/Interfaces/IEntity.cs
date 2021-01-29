using Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface IEntity
    {
        string Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        string ExternalId { get; set; }
        IEnumerable<CustomAttribute> CustomAttributes { get; set; }
    }
}
