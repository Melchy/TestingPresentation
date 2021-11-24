using System.Threading;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace WebApplication
{
    public interface IHumbleSmtpClient
    {
        void Connect(
            string host,
            int port,
            bool useSsl,
            CancellationToken cancellationToken = default);

        void Send(
            MimeMessage message,
            CancellationToken cancellationToken = default,
            ITransferProgress progress = null);

        void Disconnect(
            bool quit,
            CancellationToken cancellationToken = default);
    }

    public class HumbleSmtpClient : IHumbleSmtpClient
    {
        private readonly SmtpClient _smtpClient;

        public HumbleSmtpClient(
            SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public void Connect(
            string host,
            int port,
            bool useSsl,
            CancellationToken cancellationToken = default)
        {
            _smtpClient.Connect(host, port, useSsl, cancellationToken);
        }

        public virtual void Send(
            MimeMessage message,
            CancellationToken cancellationToken = default,
            ITransferProgress progress = null)
        {
            _smtpClient.Send(message, cancellationToken, progress);
        }

        public void Disconnect(
            bool quit,
            CancellationToken cancellationToken = default)
        {
            _smtpClient.Disconnect(quit, cancellationToken);
        }
    }

    public class EmailService
    {
        private readonly IHumbleSmtpClient _smtpClient;

        public EmailService(
            IHumbleSmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public void SendRegistrationEmail(
            string receiver)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("sender@notino.com"));
            message.To.Add(new MailboxAddress(receiver));
            message.Subject = "Subject";
            message.Body = new TextPart("plain")
            {
                Text = "Body",
            };

            _smtpClient.Connect("smtp.friends.com", 587, false);
            _smtpClient.Send(message);
            _smtpClient.Disconnect(true);
        }
    }
}
