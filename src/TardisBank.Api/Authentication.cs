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
        public static string CreateToken(string encryptionKey, Login login)
        {
            if(encryptionKey == null) throw new ArgumentNullException(encryptionKey);
            if(login == null) throw new ArgumentNullException(nameof(login));

            var tes = new TripleDESCryptoServiceProvider();
            tes.Key = Convert.FromBase64String(encryptionKey);

            var jsonBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Login
            {
                Email = login.Email,
                LoginId = login.LoginId,
                PasswordHash = string.Empty
            }));

            var encriptor = tes.CreateEncryptor();
            var resultBytes = encriptor.TransformFinalBlock(jsonBytes, 0, jsonBytes.Length);
            var resultBase64 = Convert.ToBase64String(resultBytes);
            var iv = Convert.ToBase64String(tes.IV);
            tes.Clear();

            return $"{resultBase64}:{iv}";
        }

        public static Maybe<Login> DecryptToken(string encryptionKey, string token)
        {
            if(encryptionKey == null) throw new ArgumentNullException(encryptionKey);
            if(token == null) throw new ArgumentNullException(nameof(token));

            var tokenParts = token.Split(':');
            if(tokenParts.Length != 2)
            {
                return Maybe<Login>.Empty();
            }

            var encodedBytes = Convert.FromBase64String(tokenParts[0]);
            var iv = Convert.FromBase64String(tokenParts[1]);

            var tes = new TripleDESCryptoServiceProvider();
            tes.Key = Convert.FromBase64String(encryptionKey);
            tes.IV = iv;

            var decryptor = tes.CreateDecryptor();
            var jsonBytes = decryptor.TransformFinalBlock(encodedBytes, 0, encodedBytes.Length);
            var json = Encoding.UTF8.GetString(jsonBytes);

            var login = JsonConvert.DeserializeObject<Login>(json);
            
            return login;
        }
    }
}