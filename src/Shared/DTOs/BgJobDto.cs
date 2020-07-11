namespace Shared.DTOs
{
    public class BgJobDto : BaseDto
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }

        public int? BgScheduleId { get; set; }
    }
}
