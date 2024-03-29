﻿using System;
using System.Collections.Generic;

namespace Shared.DTOs
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual DateTime DateUpdated { get; set; } = DateTime.Now;
        public virtual string CreatedBy { get; set; } = "?";
        public virtual string UpdatedBy { get; set; } = "?";
        public virtual string ExternalId { get; set; }

        public string DateUpdatedFormatted { get { return DateUpdated.ToString("MM/dd/yyyy"); } }

        public IEnumerable<CustomAttributeDto> CustomAttributes { get; set; }
    }
}
