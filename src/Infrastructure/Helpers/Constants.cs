namespace Infrastructure.Helpers
{
    public static class Constants
    {
        public static class Claims
        {
            public static string FirstName = "FirstName";
            public static string LastName = "LastName";
            public static string Email = "Email";
            public static string UserId = "UserId";
            public static string Roles = "Roles";
        }

        public static class BackgroundTaskStatus
        {
            public static string Ready = "Ready";
            public static string Started = "Started";
        }
    }
}
