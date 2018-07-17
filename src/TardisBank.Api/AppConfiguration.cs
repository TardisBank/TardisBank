using System;

namespace TardisBank.Api
{
    public class AppConfiguration
    {
        public string ConnectionString { get; }
        public string EncryptionKey { get; }
        public string SmtpServer { get; }
        public string SmtpCredentials { get; }

        private AppConfiguration(
            string connectionString, 
            string encryptionKey,
            string smtpServer,
            string smtpCredentials)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            EncryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));
            SmtpServer = smtpServer ?? throw new ArgumentNullException(nameof(smtpServer));
            SmtpCredentials = smtpCredentials ?? throw new ArgumentNullException(nameof(smtpCredentials));
        }

        public static AppConfiguration LoadFromEnvironment()
        {
            var connectionString = Environment.GetEnvironmentVariable("TARDISBANK_DB_CON");
            var encryptionKey = Environment.GetEnvironmentVariable("TARDISBANK_KEY");
            var smtpServer = Environment.GetEnvironmentVariable("TARDISBANK_SMTP_SERVER");
            var smtpCredentials = Environment.GetEnvironmentVariable("TARDISBANK_SMTP_CREDENTIALS");

            return new AppConfiguration(
                connectionString, 
                encryptionKey, 
                smtpServer,
                smtpCredentials);
        }

        public EmailConfiguration GetEmailConfiguration()
        {
            var serverParts = SmtpServer.Split(':');
            int serverPort = 0;
            const string errorMessage = "Misconfigured TARDISBANK_SMTP_SERVER. Expected '<server>:<port>:<use HTTPS>[true|false]'.";
            if(serverParts.Length != 3)
            {
                throw new ApplicationException(errorMessage);
            }
            if(!int.TryParse(serverParts[1], out serverPort))
            {
                throw new ApplicationException(errorMessage + " Could not parse <port> as integer.");
            }
            if(serverParts[2] != "true" && serverParts[2] != "false")
            {
                throw new ApplicationException(errorMessage + " Could not parse <use HTTPS> as bool, expected 'true' or 'false'.");
            }
            var useSLL = serverParts[2] == "true";


            var credentialParts = SmtpCredentials.Split(':');
            if(credentialParts.Length != 2)
            {
                throw new ApplicationException("Misconfigured TARDISBANK_SMTP_CREDENTIALS. Expected '<username>:<password>'.");
            }

            return new EmailConfiguration(
                serverParts[0], 
                serverPort, 
                useSLL, 
                credentialParts[0], 
                credentialParts[1]);
        }
    }

    public class EmailConfiguration
    {
        public string SmtpServer { get; }
        public int SmtpServerPort { get; }
        public bool UseSSL { get; }
        public string SmtpUsername { get; }
        public string SmtpPassword { get; }

        public EmailConfiguration(
            string smtpServer,
            int smtpServerPort,
            bool useSSL,
            string smtpUsername,
            string smtpPassword)
        {
            SmtpServer = smtpServer ?? throw new ArgumentNullException(nameof(smtpServer));
            SmtpServerPort = smtpServerPort;
            UseSSL = useSSL;
            SmtpUsername = smtpUsername ?? throw new ArgumentNullException(nameof(smtpUsername));
            SmtpPassword = smtpPassword ?? throw new ArgumentNullException(nameof(smtpPassword));
        }
    }
}