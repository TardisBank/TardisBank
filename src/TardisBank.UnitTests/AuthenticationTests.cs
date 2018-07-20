using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using E247.Fun;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Moq;
using TardisBank.Api;
using Xunit;

namespace TardisBank.UnitTests
{
    public class AuthenticationTests
    {
        [Fact(Skip = "Don't generate key file on each test run.")]
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

        [Fact]
        public async Task AuthenticateShouldPopulateUserOnSuccess()
        {
            var request = new Mock<HttpRequest>();
            var response = new Mock<HttpResponse>();
            var context = new Mock<HttpContext>();
            var headers = new HeaderDictionary();
            var items = new Dictionary<object, object>();

            context.Setup(x => x.Request).Returns(request.Object);
            context.Setup(x => x.Response).Returns(response.Object);
            context.Setup(x => x.Items).Returns(items);
            request.Setup(x => x.Headers).Returns(headers);

            headers.Add("Authorization", "Bearer my-token");
            var expectedLogin = new Login 
            {
                LoginId = 12,
                Email = "bob@email.com"
            };
            bool nextWasCalled = false;
            Func<Task> next = () => 
            {
                nextWasCalled = true;
                return Task.FromResult(0);
            };

            var authenticate = Authentication.Authenticate(x => expectedLogin);
            await authenticate(context.Object, next);

            Assert.True(nextWasCalled);
            Assert.Equal(expectedLogin, context.Object.GetAuthenticatedLogin());
        }

        [Fact]
        public async Task AuthenticateShouldCallNextWhenNoAuthHeaderExists()
        {
            var request = new Mock<HttpRequest>();
            var response = new Mock<HttpResponse>();
            var context = new Mock<HttpContext>();
            var headers = new HeaderDictionary();
            var items = new Dictionary<object, object>();

            context.Setup(x => x.Request).Returns(request.Object);
            context.Setup(x => x.Response).Returns(response.Object);
            context.Setup(x => x.Items).Returns(items);
            request.Setup(x => x.Headers).Returns(headers);

            var expectedLogin = new Login();
            bool nextWasCalled = false;
            Func<Task> next = () => 
            {
                nextWasCalled = true;
                return Task.FromResult(0);
            };

            var authenticate = Authentication.Authenticate(x => expectedLogin);
            await authenticate(context.Object, next);

            Assert.True(nextWasCalled);
            Assert.False(context.Object.IsAuthenticated());
        }

        private static string GenerateKey()
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