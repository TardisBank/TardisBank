using System;
using Npgsql;

namespace TardisBank.IntegrationTests
{
    public static class DbHelper
    {
        public static void UpdateEmailVerified(string email)
        {
            var connectionString = Environment.GetEnvironmentVariable("TARDISBANK_DB_CON");

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"UPDATE login SET verified = TRUE WHERE email = @Email";
                    cmd.Parameters.AddWithValue("Email", email);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}