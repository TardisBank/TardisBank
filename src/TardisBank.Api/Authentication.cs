using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using E247.Fun;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TardisBank.Dto;

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

        const string authHeaderName = "Authorization";
        const string contextLoginKey = "tardisbank.login";

        public static Func<HttpContext, Func<Task>, Task> Authenticate(Func<string, Maybe<Login>> decryptToken)
        {
            return async (context, next) =>
            {
                if(!context.Request.Headers.ContainsKey(authHeaderName))
                {
                    await next();
                    return;
                }

                var authHeader = context.Request.Headers[authHeaderName];
                if(authHeader.Count > 0 && authHeader[0].StartsWith("Bearer"))
                {
                    var headerParts = authHeader[0].Split(' ');
                    if(headerParts.Length == 2)
                    {
                        decryptToken(headerParts[1]).Match(
                            Some: login => 
                            { 
                                var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]{
                                    new Claim("Email", login.Email),
                                    new Claim("loginId", login.LoginId.ToString())
                                }));
                                context.User = principal;
                                context.Items.Add(contextLoginKey, login);
                            },
                            None: () => {}
                        );
                    }
                }

                if(context.IsAuthenticated())
                {
                    await next();
                }
                else
                {
                    context.Response.StatusCode = 401;
                    var json = JsonConvert.SerializeObject(new ErrorResponse 
                    {
                        Message = "Forbidden"
                    });
                    await context.Response.WriteAsync(json);
                }
            };
        }

        public static bool IsAuthenticated(this HttpContext context)
            => context.Items.ContainsKey(contextLoginKey);

        public static Maybe<Login> GetAuthenticatedLogin(this HttpContext context)
            => IsAuthenticated(context) 
                ? context.Items[contextLoginKey] as Login
                : Maybe<Login>.Empty();

        private static readonly Maybe<Login> loginNothing = Maybe<Login>.Empty();
    }
}