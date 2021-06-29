using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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

        // https://damienbod.com/2020/08/19/symmetric-and-asymmetric-encryption-in-net-core/
        public static (string Key, string IVBase64) InitSymmetricEncryptionKeyIV()
        {
            var key = GetEncodedRandomString(32); // 256
            Aes cipher = CreateCipher(key);
            var IVBase64 = Convert.ToBase64String(cipher.IV);
            return (key, IVBase64);
        }

        private static string GetEncodedRandomString(int length)
        {
            var base64 = Convert.ToBase64String(GenerateRandomBytes(length));
            return base64;
        }

        private static Aes CreateCipher(string keyBase64)
        {
            // Default values: Keysize 256, Padding PKC27
            Aes cipher = Aes.Create();
            cipher.Mode = CipherMode.CBC;  // Ensure the integrity of the ciphertext if using CBC

            cipher.Padding = PaddingMode.ISO10126;
            cipher.Key = Convert.FromBase64String(keyBase64);

            return cipher;
        }

        private static byte[] GenerateRandomBytes(int length)
        {
            var byteArray = new byte[length];
            RandomNumberGenerator.Fill(byteArray);
            return byteArray;
        }

        public static string Encrypt(string text, string IV, string key)
        {
            Aes cipher = CreateCipher(key);
            cipher.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return Convert.ToBase64String(cipherText);
        }

        public static string Decrypt(string encryptedText, string IV, string key)
        {
            Aes cipher = CreateCipher(key);
            cipher.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
