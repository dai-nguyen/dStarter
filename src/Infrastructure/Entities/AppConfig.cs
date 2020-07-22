namespace Infrastructure.Entities
{
    public class AppConfig : BaseEntity
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
