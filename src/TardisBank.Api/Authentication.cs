using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using E247.Fun;

namespace TardisBank.Api
{
    public static class Authentication
    {
        private static readonly TimeSpan tokenPeriod = TimeSpan.FromHours(1);

        public static string CreateToken(string encryptionKey, Func<DateTimeOffset> now, Login login)
        {
            if(encryptionKey == null) throw new ArgumentNullException(encryptionKey);
            if(login == null) throw new ArgumentNullException(nameof(login));

            var tes = new TripleDESCryptoServiceProvider();
            tes.Key = Convert.FromBase64String(encryptionKey);

            var jsonBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                new Token
                {
                    Login = new Login
                    {
                        Email = login.Email,
                        LoginId = login.LoginId,
                        PasswordHash = string.Empty
                    },
                    Expires = now()
                }));

            var encriptor = tes.CreateEncryptor();
            var resultBytes = encriptor.TransformFinalBlock(jsonBytes, 0, jsonBytes.Length);
            var resultBase64 = Convert.ToBase64String(resultBytes);
            var iv = Convert.ToBase64String(tes.IV);
            tes.Clear();

            return $"{resultBase64}:{iv}";
        }

        public static Maybe<Login> DecryptToken(string encryptionKey, Func<DateTimeOffset> now, string token)
        {
            if(encryptionKey == null) throw new ArgumentNullException(encryptionKey);
            if(token == null) throw new ArgumentNullException(nameof(token));

            var tokenParts = token.Split(':');
            if(tokenParts.Length != 2)
            {
                return loginNothing;
            }

            var encodedBytes = Convert.FromBase64String(tokenParts[0]);
            var iv = Convert.FromBase64String(tokenParts[1]);

            var tes = new TripleDESCryptoServiceProvider();
            tes.Key = Convert.FromBase64String(encryptionKey);
            tes.IV = iv;

            string json = null;
            try
            {
                var decryptor = tes.CreateDecryptor();
                var jsonBytes = decryptor.TransformFinalBlock(encodedBytes, 0, encodedBytes.Length);
                json = Encoding.UTF8.GetString(jsonBytes);
            }
            catch(System.Security.Cryptography.CryptographicException)
            {
                return loginNothing;
            }

            var tokenModel = JsonConvert.DeserializeObject<Token>(json);
            if(tokenModel.Expires.Add(tokenPeriod) < now())
            {
                return loginNothing;
            }
            
            return tokenModel.Login;
        }

        private static readonly Maybe<Login> loginNothing = Maybe<Login>.Empty();
    }
}