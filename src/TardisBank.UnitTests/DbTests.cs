using System;
using System.Threading.Tasks;
using Npgsql;
using TardisBank.Api;
using Xunit;
using Xunit.Abstractions;

namespace TardisBank.IntegrationTests
{
    public class DbTests
    {
        private readonly ITestOutputHelper output;
        private readonly string connectionString;

        public DbTests(ITestOutputHelper output)
        {
            this.output = output;
            connectionString = Environment.GetEnvironmentVariable("TARDISBANK_DB_CON");
        }

        [Fact]
        public async Task ShouldBeAbleToInsertLogin()
        {
            var login = new Login
            {
                Email = $"{Guid.NewGuid().ToString()}@mailinator.com",
                PasswordHash = Guid.NewGuid().ToString()
            };

            var returnedLogin = await Db.InsertLogin(connectionString, login);

            Assert.Equal(login.Email, returnedLogin.Email);
            Assert.Equal(login.PasswordHash, returnedLogin.PasswordHash);
            Assert.True(returnedLogin.LoginId > 0);
        }

        [Fact]
        public async Task ShouldBeAbleToGetLoginByEmail()
        {
            var login = new Login
            {
                Email = $"{Guid.NewGuid().ToString()}@mailinator.com",
                PasswordHash = Guid.NewGuid().ToString()
            };

            await Db.InsertLogin(connectionString, login);
            var result = await Db.LoginByEmail(connectionString, login.Email);

            Assert.True(result.HasValue);
            var returnedLogin = result.Value;
            Assert.Equal(login.Email, returnedLogin.Email);
            Assert.Equal(login.PasswordHash, returnedLogin.PasswordHash);
            Assert.True(returnedLogin.LoginId > 0);
        }

        [Fact]
        public async Task ShouldBeAbleToInsertSelectAndDeleteAccount()
        {
            var login = new Login
            {
                Email = $"{Guid.NewGuid().ToString()}@mailinator.com",
                PasswordHash = Guid.NewGuid().ToString()
            };

            var returnedLogin = await Db.InsertLogin(connectionString, login);

            var account = new Account
            {
                LoginId = returnedLogin.LoginId,
                AccountName = Guid.NewGuid().ToString()
            };

            var returnedAccount = await Db.InsertAccount(connectionString, account);

            Assert.Equal(account.LoginId, returnedAccount.LoginId);
            Assert.Equal(account.AccountName, returnedAccount.AccountName);
            Assert.True(returnedAccount.AccountId > 0);

            {
                var accounts = await Db.AccountByLogin(connectionString, returnedLogin);
                Assert.Collection(accounts, 
                    x => Assert.Equal(returnedAccount.AccountId, x.AccountId));
            }

            await Db.DeleteAccount(connectionString, returnedAccount);

            {
                var accounts = await Db.AccountByLogin(connectionString, returnedLogin);
                Assert.Collection(accounts);
            }
        }
    }
}