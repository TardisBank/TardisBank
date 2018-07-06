using System.Threading.Tasks;
using Npgsql;
using Dapper;
using System.Linq;

namespace TardisBank.Api
{
    public static class Db
    {
        public static async Task<Login> InsertLogin(string connectionString, Login login)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var result = await conn.QueryAsync<Login>(
                    "INSERT INTO login (email, password_hash) " + 
                    "VALUES (@Email, @PasswordHash) " + 
                    "RETURNING login_id as LoginId, email as Email, password_hash as PasswordHash",
                    login);

                return result.Single();
            }
        }
    }
}
