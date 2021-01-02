using System;

namespace Shared.DTOs
{
    public class LogMsgTableOptionDto : TableOptionDto
    {
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public string[] Levels { get; set; }
    }
}
