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
        public void SendRegistrationEmail(string receiver){
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress ("sender@notino.com"));
            message.To.Add (new MailboxAddress (receiver));
            message.Subject = "...";
            message.Body = new TextPart ("plain") {
                Text = "...."
            };
            using (var client = new SmtpClient ()) {
                client.Connect ("smtp.friends.com", 587, false);
                client.Send (message);
                client.Disconnect (true);
            }
        }
    }
}
