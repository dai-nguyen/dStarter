using System.Collections.Generic;

namespace Shared.DTOs
{
    public class WikiDto : BaseDto
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public string[] Tags { get; set; }
    }
}
