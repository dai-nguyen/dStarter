using System;

namespace Shared.DTOs
{
    public class LogMsgDto
    {
        public string message { get; set; }
        public string message_template { get; set; }
        public string level { get; set; }
        public DateTime raise_date { get; set; }
        public string raise_date_formatted { get { return raise_date.ToString("MM/dd/yyyy h:mm tt"); } }
        public string exception { get; set; }
        public string properties { get; set; }
        public string props_test { get; set; }
        public string machine_name { get; set; }
        public string user_name { get; set; }
    }
}
