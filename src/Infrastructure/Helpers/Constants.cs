using Infrastructure.Data;

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

        public static class ParentNames
        {
            public static string User = nameof(AppUser);
            public static string Role = nameof(AppRole);
        }

        public static class DataTypes
        {
            public static string Numeric = nameof(Numeric);
            public static string Text = nameof(Text);
            public static string Date = nameof(Date);
        }
    }
}
