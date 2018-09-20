using System;
using Npgsql;

namespace TardisBank.IntegrationTests
{
    public static class DbHelper
    {
        public static void UpdateEmailVerified(string email)
        {
            //var connectionString = Environment.GetEnvironmentVariable("TARDISBANK_DB_CON");
            var connectionString = "Server=127.0.0.1;Port=5432;Database=tardisbank;User Id=tardisbank_app;Password=_wHaT3v3R;";

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