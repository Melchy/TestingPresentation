using MailKit.Net.Smtp;
using MimeKit;

namespace WebApplication
{
    public class EmailService
    {
        public void SendRegistrationEmail(string reciever){
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress ("sender@notino.com"));
            message.To.Add (new MailboxAddress (reciever));
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
