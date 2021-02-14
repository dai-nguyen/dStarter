using Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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
        public void TestEncryptDecrypt()
        {
            var text = "this is just a test";
            //var symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
            var (Key, IVBase64) = Helper.InitSymmetricEncryptionKeyIV();

            var encrypted = Helper.Encrypt(text, IVBase64, Key);
            var decrypted = Helper.Decrypt(encrypted, IVBase64, Key);


            Assert.IsTrue(text == decrypted);
        }

        [TestMethod]
        public void ReadCertFile()
        {
            var bytes = File.ReadAllBytes("C:\\Web.pfx");
            var str = Convert.ToBase64String(bytes);
        }

        [TestMethod]
        public void LoadCertFile()
        {
            try
            {
                var cert = new X509Certificate2("C:\\Web.pfx", "LetMeSurf");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
