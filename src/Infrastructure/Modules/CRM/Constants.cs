using System;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Modules.CRM
{
    public class Constants
    {
        public static class TicketStatus
        {
            public static string Open = "Open";
            public static string Closed = "Closed";
            public static string Cancel = "Cancel";
            public static string Completed = "Completed";
        }

        public static string[] TicketStatuses()
        {
            Type t = typeof(TicketStatus);
            var fields = t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty);
            return fields.Select(_ => _.GetValue(null).ToString()).ToArray();
        }
    }
}
