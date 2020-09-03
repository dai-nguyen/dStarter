namespace Infrastructure.Entities
{
    public class CustomAttribute : BaseEntity
    {
        public string Value { get; set; }

        public int DefinitionId { get; set; }
        public int ParentId { get; set; }
        public CustomAttributeDefinition Definition { get; set; }
    }
}
