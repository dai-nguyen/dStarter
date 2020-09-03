using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class CustomAttributeDefinition : BaseEntity
    {
        public string ObjectName { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DataType { get; set; }

        public IEnumerable<CustomAttribute> CustomAttributes { get; set; }
    }
}
