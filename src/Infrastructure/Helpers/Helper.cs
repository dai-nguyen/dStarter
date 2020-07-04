using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Infrastructure.Helpers
{
    public static class Helper
    {
        public static string CurrentFolder = 
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
