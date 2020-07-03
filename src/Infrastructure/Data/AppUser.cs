﻿using Microsoft.AspNetCore.Identity;
using System;

namespace Infrastructure.Data
{
    public class AppUser : IdentityUser
    {
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual DateTime DateUpdated { get; set; } = DateTime.Now;
        public virtual string CreatedBy { get; set; } = "?";
        public virtual string UpdatedBy { get; set; } = "?";
        public virtual string ExternalId { get; set; }

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
