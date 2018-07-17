using System;
using System.Net;
using System.Net.Mail;

namespace TardisBank.Api
{
    public static class Email
    {
        public static void Send(EmailConfiguration configuration, EmailMessage email)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            if (string.IsNullOrWhiteSpace(configuration.SmtpServer)) return;

            var message = new MailMessage(
                        configuration.SmtpUsername,
                        email.ToAddress,
                        email.Subject,
                        email.Body);

            var client = new SmtpClient(configuration.SmtpServer)
            {
                EnableSsl = configuration.UseSSL,
                Port = configuration.SmtpServerPort,
            };

            if (!string.IsNullOrWhiteSpace(configuration.SmtpUsername) && 
                !string.IsNullOrWhiteSpace(configuration.SmtpPassword))
            {
                client.Credentials = new NetworkCredential(
                    configuration.SmtpUsername,
                    configuration.SmtpPassword);
            }

            client.Send(message);            
        }
    }
}