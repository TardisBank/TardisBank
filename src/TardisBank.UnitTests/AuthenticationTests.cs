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
        public void GenerateKeyFile()
        {
            var encodedKey = GenerateKey();

            File.WriteAllText(@"C:\Users\mikeh\tardis-key.txt", encodedKey);
        }

        [Fact]
        public void ShouldBeAbleToEncryptAndDecryptToken()
        {
            var encryptionKey = GenerateKey();

            var token = Authentication.CreateToken(encryptionKey, () => now, login);
            var returnedLogin = Authentication.DecryptToken(encryptionKey, () => now, token);

            Assert.True(returnedLogin.HasValue);
            Assert.Equal(login.Email, returnedLogin.Value.Email);
            Assert.Equal(login.LoginId, returnedLogin.Value.LoginId);
            Assert.Equal("", returnedLogin.Value.PasswordHash);
        }

        [Fact]
        public void TokenValidationShouldFailIfExpired()
        {
            var encryptionKey = GenerateKey();
            var twoHoursLater = now.AddHours(2);

            var token = Authentication.CreateToken(encryptionKey, () => now, login);
            var returnedLogin = Authentication.DecryptToken(encryptionKey, () => twoHoursLater, token);

            Assert.False(returnedLogin.HasValue);
        }

        [Fact]
        public void TokenValidationShouldFailIfWrongKeyIsUsed()
        {
            var encryptionKey = GenerateKey();
            var incorrectDecryptionKey = GenerateKey();

            var token = Authentication.CreateToken(encryptionKey, () => now, login);
            var returnedLogin = Authentication.DecryptToken(incorrectDecryptionKey, () => now, token);

            Assert.False(returnedLogin.HasValue);
        }

        [Fact]
        public void TokenValidationShouldFailIfTokenIsInvalid()
        {
            var encryptionKey = GenerateKey();
            var decryptionKey = GenerateKey();

            var token = "invliad token";
            var returnedLogin = Authentication.DecryptToken(decryptionKey, () => now, token);

            Assert.False(returnedLogin.HasValue);
        }

        private string GenerateKey()
            => Convert.ToBase64String(new TripleDESCryptoServiceProvider().Key);

        private Login login = new Login
        {
            Email = "foo@bar.com",
            PasswordHash = "xyz",
            LoginId = 101
        };

        DateTimeOffset now = new DateTimeOffset(2018, 7, 9, 0, 0, 0, TimeSpan.FromTicks(0));
    }
}