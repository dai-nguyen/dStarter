using Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
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

        [TestMethod]
        public void TestEncryption()
        {
            var key = Convert.ToBase64String(GetRandomData(128));

            //key = key.Replace("==", "@#");

            var encrypted = Helper.EncryptString("this is a sensitive value", key);
            var decrypt = Helper.DecryptString(encrypted, key);

            Assert.IsTrue(decrypt == "hello");
        }

        private byte[] GetRandomData(int bits)
        {
            var result = new byte[bits / 8];
            RandomNumberGenerator.Create().GetBytes(result);
            return result;
        }
    }
}
