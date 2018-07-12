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
                x => Assert.Equal(Rels.Self, x.Rel),
                x => Assert.Equal(Rels.Home, x.Rel));
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
                x => Assert.Equal(Rels.Account, x.Rel),
                x => Assert.Equal(Rels.Self, x.Rel),
                x => Assert.Equal(Rels.Home, x.Rel));

            Assert.Equal(email, home.Email);
        }

        [Fact]
        public async Task DeleteLoginShouldWork()
        {
            var email = $"{Guid.NewGuid().ToString()}@mailinator.com";
            var password = Guid.NewGuid().ToString();

            var authenticatedClient = await RegisterAndLogin(email, password);
            var home = await authenticatedClient.GetHome();

            await authenticatedClient.Delete<HomeResponse>(home.Link(Rels.Self));

            var unauthenticatedHome = await client.GetHome();

            // attempt to login...
            var login = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var loginResult = await client.Post<LoginRequest, LoginResponse>(unauthenticatedHome.Link(Rels.Login), login);
        }

        [Fact]
        public async Task PostAndGetAccountShouldWork()
        {
            var authenticatedClient = await RegisterAndLogin();
            var home = await authenticatedClient.GetHome();

            var account = new AccountRequest
            {
                AccountName = Guid.NewGuid().ToString()
            };

            var returnedAccount = await authenticatedClient.Post<AccountRequest, AccountResponse>(home.Link(Rels.Account), account);
            Assert.Equal(account.AccountName, returnedAccount.AccountName);

            {
                var result = await authenticatedClient.Get<AccountResponseCollection>(home.Link(Rels.Account));
                Assert.Collection(result.Accounts, 
                    x => Assert.Equal(account.AccountName, x.AccountName));
            }

            {
                var result = await authenticatedClient.Delete<AccountResponse>(returnedAccount.Link(Rels.Self));
            }

            {
                var result = await authenticatedClient.Get<AccountResponseCollection>(home.Link(Rels.Account));
                Assert.Collection(result.Accounts);
            }
        }

        [Fact]
        public async Task PostAndGetTransactionShouldWork()
        {
            var authenticatedClient = await RegisterAndLogin();
            var home = await authenticatedClient.GetHome();

            var account = new AccountRequest
            {
                AccountName = Guid.NewGuid().ToString()
            };

            var returnedAccount = await authenticatedClient.Post<AccountRequest, AccountResponse>(home.Link(Rels.Account), account);

            var transaction1 = await authenticatedClient.Post<TransactionRequest, TransactionResponse>(
                returnedAccount.Link(Rels.Transaction), new TransactionRequest { Amount = 1.45M });

            var transaction2 = await authenticatedClient.Post<TransactionRequest, TransactionResponse>(
                returnedAccount.Link(Rels.Transaction), new TransactionRequest { Amount = 2.78M });

            Assert.Equal(1.45M, transaction1.Amount);
            Assert.Equal(1.45M, transaction1.Balance);
            Assert.Equal(DateTimeOffset.Now.Day, transaction1.TransactionDate.Day);
            Assert.Equal(2.78M, transaction2.Amount);
            Assert.Equal(2.78M + 1.45M, transaction2.Balance);

            var transactions = await authenticatedClient.Get<TransactionResponseCollection>(
                returnedAccount.Link(Rels.Transaction));
            
            Assert.Collection(transactions.Transactions,
                x => Assert.Equal(transaction2.Amount, x.Amount),
                x => Assert.Equal(transaction1.Amount, x.Amount));
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
