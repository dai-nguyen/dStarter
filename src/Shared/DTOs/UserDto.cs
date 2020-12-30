using System;
using System.Collections.Generic;

namespace Shared.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual DateTime DateUpdated { get; set; } = DateTime.Now;
        public virtual string CreatedBy { get; set; } = "?";
        public virtual string UpdatedBy { get; set; } = "?";
        public virtual string ExternalId { get; set; }


        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<CustomAttributeDto> CustomAttributes { get; set; }
    }
}
