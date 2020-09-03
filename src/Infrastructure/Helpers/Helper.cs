using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static string[] GetParentNames()
        {
            var type = typeof(Constants.ParentNames);
            FieldInfo[] fields = type.GetFields(
                BindingFlags.Public | BindingFlags.Static);

            return fields.Select(_ => _.Name)
                .OrderBy(_ => _)
                .ToArray();
        }

        public static string[] GetDataTypes()
        {
            var type = typeof(Constants.DataTypes);
            FieldInfo[] fields = type.GetFields(
                BindingFlags.Public | BindingFlags.Static);

            return fields.Select(_ => _.Name)
                .OrderBy(_ => _)
                .ToArray();
        }
    }
}
