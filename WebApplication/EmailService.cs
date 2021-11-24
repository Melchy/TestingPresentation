using MailKit.Net.Smtp;
using MimeKit;

namespace WebApplication
{
    public interface IEmailService
    {
        public void SendRegistrationEmail(
            string receiver);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(
            SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public void SendRegistrationEmail(
            string receiver)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("sender@notino.com"));
            message.To.Add(new MailboxAddress(receiver));
            message.Subject = "...";
            message.Body = new TextPart("plain")
            {
                Text = "....",
            };

            _smtpClient.Connect("smtp.friends.com", 587, false);
            _smtpClient.Send(message);
            _smtpClient.Disconnect(true);
        }
    }
}
