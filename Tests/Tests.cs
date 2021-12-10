using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using MailKit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using NUnit.Framework;
using WebApplication;

namespace Tests
{
    public class Tests
    {
        [Test]
        public async Task UserRegistrationSimple()
        {
            var application = new WebApplicationFactory<Program>();
            var userRepository = A.Fake<IUserRepository>();
            var smtpClientMock = new SmtpClientMock();
            application = application
               .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(x => x.AddTransient<IHumbleSmtpClient, SmtpClientMock>(x => smtpClientMock));
                    builder.ConfigureServices(x => x.AddTransient(x => userRepository));
                });
            var client = application.CreateClient();
            var result = await client.PostAsJsonAsync("/User", new User("reciever@email.com", "name"));

            result.EnsureSuccessStatusCode();
            smtpClientMock.Message.To.Single().ToString().Should().Be("reciever@email.com");
            ((TextPart)smtpClientMock.Message.Body).Text.Should().Be("Body");
            smtpClientMock.Message.Subject.Should().Be("Subject");
            smtpClientMock.Message.From.Single().ToString().Should().Be("sender@notino.com");
        }
    }



    public class SmtpClientMock : IHumbleSmtpClient
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public int NumberOfCallsToConnect { get; set; }

        public MimeMessage Message { get; set; }
        public int NumberOfCallsToSend { get; set; }

        public bool Quit { get; set; }
        public int NumberOfCallsToDisconnect { get; set; }

        public void Connect(
            string host,
            int port,
            bool useSsl,
            CancellationToken cancellationToken = default)
        {
            NumberOfCallsToConnect++;
            Host = host;
            Port = port;
        }

        public void Send(
            MimeMessage message,
            CancellationToken cancellationToken = default,
            ITransferProgress progress = null)
        {
            NumberOfCallsToSend++;
            Message = message;
        }

        public void Disconnect(
            bool quit,
            CancellationToken cancellationToken = default)
        {
            NumberOfCallsToDisconnect++;
            Quit = quit;
        }
    }
}
