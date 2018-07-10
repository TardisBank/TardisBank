using System;

namespace TardisBank.Api
{
    public class AppConfiguration
    {
        public string ConnectionString { get; }
        public string EncryptionKey { get; }

        private AppConfiguration(string connectionString, string encryptionKey)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            EncryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));
        }

        public static AppConfiguration LoadFromEnvironment()
        {
            var connectionString = Environment.GetEnvironmentVariable("TARDISBANK_DB_CON");
            var encryptionKey = Environment.GetEnvironmentVariable("TARDISBANK_KEY");

            return new AppConfiguration(connectionString, encryptionKey);
        }

    }
}