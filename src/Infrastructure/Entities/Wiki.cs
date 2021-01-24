using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class Wiki : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public string[] Tags { get; set; }
    }
}
