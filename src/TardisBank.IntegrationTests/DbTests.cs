using System;
using System.Net.Http;
using System.Threading.Tasks;
using Npgsql;
using TardisBank.Client;
using TardisBank.Dto;
using Xunit;
using Xunit.Abstractions;

namespace TardisBank.IntegrationTests
{
    public class DbTests
    {
        private readonly ITestOutputHelper output;

        public DbTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ShouldBeAbleToConnectToDb()
        {
            var connectionString = Environment.GetEnvironmentVariable("TARDISBANK_DB_CON");
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
    }
}