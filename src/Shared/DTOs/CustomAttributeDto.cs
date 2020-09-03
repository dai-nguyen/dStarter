namespace Shared.DTOs
{
    public class CustomAttributeDto : BaseDto
    {
        public string Value { get; set; }

        public int DefinitionId { get; set; }
        public int ParentId { get; set; }
        public CustomAttributeDefinitionDto Definition { get; set; }
    }
}
