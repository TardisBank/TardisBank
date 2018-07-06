using System;
using System.Security.Cryptography;
using System.Text;

namespace TardisBank.Api
{
    public static class Password
    {
        public static string HashPassword(this string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            var saltBytes = GenerateSalt(4);

            return HashPassword(password, saltBytes);
        }

        public static bool HashMatches(this string password, string hashedPassword)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));

            var saltBytes = GetSaltBytes(hashedPassword);
            var newHash = HashPassword(password, saltBytes);

            return newHash == hashedPassword;
        }

        private static string HashPassword(string password, byte[] saltBytes)
        {
            var algorithm = new SHA1Managed();
            var plainTextBytes = Encoding.UTF8.GetBytes(password);

            var plainTextWithSaltBytes = AppendByteArray(plainTextBytes, saltBytes);
            var saltedSHA1Bytes = algorithm.ComputeHash(plainTextWithSaltBytes);
            var saltedSHA1WithAppendedSaltBytes = AppendByteArray(saltedSHA1Bytes, saltBytes);

            return Convert.ToBase64String(saltedSHA1WithAppendedSaltBytes);
        }

        private static byte[] GetSaltBytes(string hashedPassword)
        {
            var hashedBytes = Convert.FromBase64String(hashedPassword);
            return hashedBytes.AsSpan().Slice(hashedBytes.Length - 4).ToArray();
        }

        private static byte[] GenerateSalt(int saltSize)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[saltSize];
            rng.GetBytes(buff);
            return buff;
        }

        private static byte[] AppendByteArray(byte[] byteArray1, byte[] byteArray2)
        {
            var byteArrayResult =
                    new byte[byteArray1.Length + byteArray2.Length];

            for (var i = 0; i < byteArray1.Length; i++)
                byteArrayResult[i] = byteArray1[i];
            for (var i = 0; i < byteArray2.Length; i++)
                byteArrayResult[byteArray1.Length + i] = byteArray2[i];

            return byteArrayResult;
        }
    }
}