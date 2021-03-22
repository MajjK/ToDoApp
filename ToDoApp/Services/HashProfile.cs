using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ToDoApp.Services
{
    public class HashProfile
    {
        private static int saltSize = 64;

        public static bool ValidatePasswords(string password, string hashedPassword, string salt)
        {
            byte[] saltedHashPassword = GenerateSaltedHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));
            return saltedHashPassword.SequenceEqual(Convert.FromBase64String(hashedPassword));
        }

        public static string GenerateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[saltSize];
            rng.GetBytes(buff);
            return Encoding.UTF8.GetString(buff);
        }

        public static string GetSaltedHashData(string password, string salt)
        {
            byte[] saltedHashPassword = GenerateSaltedHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));
            return Convert.ToBase64String(saltedHashPassword);
        }

        private static byte[] GenerateSaltedHash(byte[] password, byte[] salt)
        {
            HashAlgorithm hashAlgorithm = new SHA512Managed();
            byte[] passwordWithSalt = new byte[password.Length + salt.Length];
            password.CopyTo(passwordWithSalt, 0);
            salt.CopyTo(passwordWithSalt, password.Length);
            return hashAlgorithm.ComputeHash(passwordWithSalt);
        }

    }
}
