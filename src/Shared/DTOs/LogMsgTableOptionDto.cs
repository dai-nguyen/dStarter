using System;

namespace Shared.DTOs
{
    public class LogMsgTableOptionDto : TableOptionDto
    {
        public string[] Usernames { get; set; }
        public DateTime Date { get; set; }
        public string[] LogLevels { get; set; }
    }
}
