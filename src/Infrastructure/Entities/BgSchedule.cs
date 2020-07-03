using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class BgSchedule : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public string Status { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int RepeatInMinute { get; set; }

        public IEnumerable<BgJob> BgJobs { get; set; }
    }
}
