using System.Threading.Tasks;
using Npgsql;
using Dapper;
using System.Linq;
using System;
using System.Data;
using E247.Fun;

namespace TardisBank.Api
{
    public static class Db
    {
        public static Task<Login> InsertLogin(string connectionString, Login login)
        {
            return WithConnection(connectionString, async conn =>
            {
                var result = await conn.QueryAsync<Login>(
                    "INSERT INTO login (email, password_hash) " + 
                    "VALUES (@Email, @PasswordHash) " + 
                    "RETURNING login_id as LoginId, email as Email, password_hash as PasswordHash",
                    login);

                return result.Single();
            });
        }

        public static Task<Maybe<Login>> LoginByEmail(string connectionString, string email)
        {
            return WithConnection<Maybe<Login>>(connectionString, 
            async conn =>
            {
                var result = await conn.QueryAsync<Login>(
                    "SELECT login_id as LoginId, email as Email, password_hash as PasswordHash " + 
                    "FROM login WHERE email = @Email",
                    new { Email = email });

                if(result.Any())
                {
                    return result.Single();
                }
                return Maybe<Login>.Empty();
            });
        }

        private async static Task<T> WithConnection<T>(
            string connectionString, 
            Func<IDbConnection, Task<T>> connectionFunction)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                return await connectionFunction(conn);
            }
        }
    }
}
