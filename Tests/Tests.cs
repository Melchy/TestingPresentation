using System;
using System.Linq;
using System.Threading;
using FakeItEasy;
using FluentAssertions;
using MailKit;
using MimeKit;
using NUnit.Framework;
using WebApplication;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void UserRegistrationSimple()
        {
            //TODO use webApplicationFactory https://docs.microsoft.com/cs-cz/dotnet/api/microsoft.aspnetcore.mvc.testing.webapplicationfactory-1?view=aspnetcore-6.0
            //Nuget package  Microsoft.AspNetCore.Mvc.Testing
            var smtpClient = new SmtpClientMock();
            var userRepository = A.Fake<IUserRepository>();
            var user = new User("reciever@email.com", "name");

            var emailService = new EmailService(smtpClient);
            var sut = new RegisterUserUseCase(emailService, userRepository);
            var registrationSuccessful = sut.Register(user);

            registrationSuccessful.Should().BeTrue();
            smtpClient.Message.To.Single().ToString().Should().Be(user.Email);
            ((TextPart)smtpClient.Message.Body).Text.Should().Be("Body");
            smtpClient.Message.Subject.Should().Be("Subject");
            smtpClient.Message.From.Single().ToString().Should().Be("sender@notino.com");
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
