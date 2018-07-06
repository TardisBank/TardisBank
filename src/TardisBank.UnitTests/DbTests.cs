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
        public void ShouldBeAbleToConnectToDb()
        {
            var email = $"{Guid.NewGuid().ToString()}@mailinator.com";
            var password = Guid.NewGuid().ToString();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                // Insert some data
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO login (email, password_hash) VALUES (@e, @p)";
                    cmd.Parameters.AddWithValue("e", email);
                    cmd.Parameters.AddWithValue("p", password);
                    cmd.ExecuteNonQuery();
                }

                // Retrieve all rows
                var count = 0;
                var actualEmail = "";
                using (var cmd = new NpgsqlCommand("SELECT * FROM login WHERE email = @e", conn))
                {
                    cmd.Parameters.AddWithValue("e", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            actualEmail = reader.GetString(1);
                            count++;
                        }
                    }
                }
                Assert.Equal(1, count);
                Assert.Equal(email, actualEmail);
            }
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
    }
}