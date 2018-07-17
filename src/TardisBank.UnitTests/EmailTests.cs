using TardisBank.Api;
using Xunit;

namespace TardisBank.UnitTests
{
    public class EmailTests
    {
        [Fact]
        public void ShouldBeAbleToSendAnEmail()
        {
            var emailConfiguration = AppConfiguration.LoadFromEnvironment().GetEmailConfiguration();

            var message = new EmailMessage
            {
                ToAddress = "tardis-bank@mailinator.com",
                Subject = "Hello from the Email integration test",
                Body = "Hi Mike, Congratulations, the email thingy works fine. Mike"
            };

            Email.Send(emailConfiguration, message);
        }
    }
}