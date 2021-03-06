using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Force.DeepCloner;

namespace UI.Utils
{
    public static class HashUtils
    {
        private static byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private static int BlockSize = 128;

        public static string MD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Base64Encode(plainTextBytes);
        }

        public static string Base64Encode(byte[] bytes) {
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = Base64DecodeAsBytes(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static byte[] Base64DecodeAsBytes(string base64EncodedData) {
            return Convert.FromBase64String(base64EncodedData);
        }

        public static String AESEncrypt(String source, String password)
        {
            if (String.IsNullOrEmpty(source) || String.IsNullOrEmpty(password))
                throw new ArgumentNullException();
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            //Encrypt
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = System.Security.Cryptography.MD5.Create();
            crypt.BlockSize = BlockSize;
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(password));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public static String AESDecrypt(String source, String password)
        {
            if (String.IsNullOrEmpty(source) || String.IsNullOrEmpty(password))
                throw new ArgumentNullException();
            //Decrypt
            byte[] bytes = Convert.FromBase64String(source);
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = System.Security.Cryptography.MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(password));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[bytes.Length];
                    cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    return Encoding.Unicode.GetString(decryptedBytes);
                }
            }
        }

        public static byte[] AESEncrypt(byte[] source, String password)
        {
            byte[] bytes = (byte[]) source.DeepClone();
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = System.Security.Cryptography.MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(password));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[bytes.Length];
                    cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    return decryptedBytes;
                }
            }
        }

        public static byte[] AESDecrypt(byte[] source, String password)
        {
            byte[] bytes = (byte[])source.DeepClone();
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = System.Security.Cryptography.MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(password));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[bytes.Length];
                    cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    return decryptedBytes;
                }
            }
        }
    }
}
