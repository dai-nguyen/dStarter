namespace Shared.DTOs
{
    public class AppConfigDto : BaseDto
    {
        public string Description { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Sensitive { get; set; }
    }
}
