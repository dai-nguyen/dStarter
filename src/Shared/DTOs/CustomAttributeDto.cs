namespace Shared.DTOs
{
    public class CustomAttributeDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Id { get; set; } // for ui only

        public CustomAttributeDto()
        {
            Name = "";
            Value = "";
        }

        public CustomAttributeDto(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
