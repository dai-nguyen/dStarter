namespace Shared.DTOs.CRM
{
    public class LaborDto : BaseDto
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public string Description { get; set; }

        public string TicketId { get; set; }        


        public string HourMinute 
        { 
            get
            {
                string minute;
                if (Minute < 0)
                    minute = $"0{Minute}";
                else
                    minute = Minute.ToString();

                return $"{Hour}:{minute}";
            }
        }

        public string ShortDescription
        {
            get
            {
                if (Description.Length <= 50)
                    return Description;
                else
                    return $"{Description.Substring(0, 47)}...";
            }
        }
    }
}
