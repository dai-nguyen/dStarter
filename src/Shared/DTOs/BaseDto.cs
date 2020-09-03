using System;

namespace Shared.DTOs
{
    public class BaseDto
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "?";
        public string UpdatedBy { get; set; } = "?";
        public string ExternalId { get; set; }
    }

    public class BaseWithAttrDto : BaseDto
    {
        public CustomAttributeDto[] CustomAttributes { get; set; }
    }
}
