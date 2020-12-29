namespace Infrastructure.Entities
{
    public class CustomAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public CustomAttribute()
        {
            Name = "";
            Value = "";
        }

        public CustomAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
