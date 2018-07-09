using System;
using System.IO;
using System.Security.Cryptography;
using TardisBank.Api;
using Xunit;

namespace TardisBank.UnitTests
{
    public class AuthenticationTests
    {
        [Fact]
        public void GenerateKey()
        {
            var tes = new TripleDESCryptoServiceProvider();

            var encodedKey = Convert.ToBase64String(tes.Key);

            File.WriteAllText(@"C:\Users\mikeh\tardis-key.txt", encodedKey);
        }

        [Fact]
        public void ShouldBeAbleToEncryptAndDecryptToken()
        {
            var tes = new TripleDESCryptoServiceProvider();
            var encryptionKey = Convert.ToBase64String(tes.Key);
            var login = new Login
            {
                Email = "foo@bar.com",
                PasswordHash = "xyz",
                LoginId = 101
            };

            var token = Authentication.CreateToken(encryptionKey, login);
            var returnedLogin = Authentication.DecryptToken(encryptionKey, token);

            Assert.True(returnedLogin.HasValue);
            Assert.Equal(login.Email, returnedLogin.Value.Email);
            Assert.Equal(login.LoginId, returnedLogin.Value.LoginId);
            Assert.Equal("", returnedLogin.Value.PasswordHash);
        }
    }
}