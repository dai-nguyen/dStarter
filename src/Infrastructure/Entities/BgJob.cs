using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Entities
{
    public class BgJob : BaseEntity
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }

        public int? BgScheduleId { get; set; }
        public BgSchedule BgSchedule { get; set; }
    }
}
