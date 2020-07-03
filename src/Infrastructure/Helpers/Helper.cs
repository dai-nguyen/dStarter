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

    }
}
