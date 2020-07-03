using System;

namespace Infrastructure.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        string ExternalId { get; set; }
    }
}
