using System;
using System.Net.Http;
using System.Threading.Tasks;
using TardisBank.Client;
using TardisBank.Dto;
using Xunit;

namespace TardisBank.IntegrationTests
{
    public class ApiTests
    {
        const string baseUri = "http://localhost.fiddler:5000/";
        private ClientConfig client = new ClientConfig(new Uri(baseUri), new HttpClient());

        [Fact]
        public async Task GetHomeShouldWork()
        {
            var result = await client.GetHome();

            Assert.Collection(result.Links, 
                x => Assert.Equal(Rels.Login, x.Rel),
                x => Assert.Equal(Rels.Self, x.Rel),
                x => Assert.Equal(Rels.Home, x.Rel));
        }

        [Fact]
        public async Task PostRegisterShouldWork()
        {
            var home = await client.GetHome();
            var result = await client.Post<RegisterRequest, RegisterResponse>(home.Link(Rels.Home), new RegisterRequest
            {
                Email = $"{Guid.NewGuid().ToString()}@mailinator.com",
                Password = Guid.NewGuid().ToString()
            });

            Assert.NotNull(result);
            Assert.Collection(result.Links, 
                x => Assert.Equal(Rels.Self, x.Rel));
        }

        [Fact]
        public async Task PostLoginShouldWork()
        {
            var home = await client.GetHome();
            var registration = new RegisterRequest
            {
                Email = $"{Guid.NewGuid().ToString()}@mailinator.com",
                Password = Guid.NewGuid().ToString()
            };

            var registrationResult = await client.Post<RegisterRequest, RegisterResponse>(home.Link(Rels.Home), registration);

            var login = new LoginRequest
            {
                Email = registration.Email,
                Password = registration.Password
            };

            var loginResult = await client.Post<LoginRequest, LoginResponse>(home.Link(Rels.Login), login);

            Assert.NotNull(loginResult);
        }

        [Fact]
        public async Task GetAuthentictedHomeShouldWork()
        {
            var email = $"{Guid.NewGuid().ToString()}@mailinator.com";
            var authenticatedClient = await RegisterAndLogin(email);

            var home = await authenticatedClient.GetHome();

            Assert.Collection(home.Links, 
                x => Assert.Equal(Rels.Self, x.Rel),
                x => Assert.Equal(Rels.Home, x.Rel));

            Assert.Equal(email, home.Email);
        }

        public async Task<ClientConfig> RegisterAndLogin(
            string email = null,
            string password = null)
        {
            var home = await client.GetHome();
            var registration = new RegisterRequest
            {
                Email = email ?? $"{Guid.NewGuid().ToString()}@mailinator.com",
                Password = password ?? Guid.NewGuid().ToString()
            };

            var registrationResult = await client.Post<RegisterRequest, RegisterResponse>(home.Link(Rels.Home), registration);

            var login = new LoginRequest
            {
                Email = registration.Email,
                Password = registration.Password
            };

            var loginResult = await client.Post<LoginRequest, LoginResponse>(home.Link(Rels.Login), login);

            return new ClientConfig(client.BaseUri, client.HttpClient, loginResult.Token);
        }
    }
}
