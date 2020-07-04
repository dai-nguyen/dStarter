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

        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<UserAttributeDto> Attributes { get; set; } = new List<UserAttributeDto>();
    }

    public class UserAttributeDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
