using Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Infrastructure.Helpers.Constants;

namespace UnitTest
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        public void Test()
        {
            var type = typeof(Constants.DataTypes);
            FieldInfo[] fields = type.GetFields(
                BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo p in fields)
            {
                
            }
        }
    }
}
