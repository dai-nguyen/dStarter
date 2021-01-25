using System.Collections.Generic;
using System.Linq;

namespace Shared.DTOs
{
    public class WikiDto : BaseDto
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public string[] Tags { get; set; }

        public string TagsStr
        {
            get
            {
                if (Tags == null || !Tags.Any())
                    return "";

                return string.Join(", ", Tags);
            }
        }
    }
}
