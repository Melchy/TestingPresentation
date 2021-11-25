using System;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using MailKit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MimeKit;
using NUnit.Framework;
using Ridge.CallResult.Controller.Extensions;
using Ridge.Interceptor.InterceptorFactory;
using Ridge.LogWriter;
using WebApplication;

namespace Tests
{
    public class WebApp : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(
            IHostBuilder builder)
        {
            var smtpClientMock = new SmtpClientMock();
            builder.ConfigureServices(x =>
            {
                x.AddSingleton<SmtpClientMock>(x => smtpClientMock);
                x.AddSingleton<IHumbleSmtpClient, SmtpClientMock>(x => smtpClientMock);
                x.AddTransient<IUserRepository>(x => A.Fake<IUserRepository>());
            });
            return base.CreateHost(builder);
        }
    }

    public class Tests
    {
        [Test]
        public async Task UserRegistrationSimple()
        {
            var webApp = new WebApp();
            var client = webApp.CreateClient();
            var controllerFactory = new ControllerFactory(client, webApp.Services);
            var sut = controllerFactory.CreateController<UserController>();

            var user = new User("reciever@email.com", "name");
            var registerUser = sut.RegisterUser(user);

            registerUser.IsSuccessStatusCode().Should().BeTrue();
            var smtpClient = webApp.Services.GetRequiredService<SmtpClientMock>();
            smtpClient.Message.To.Single().ToString().Should().Be(user.Email);
            ((TextPart)smtpClient.Message.Body).Text.Should().Be("Body");
            smtpClient.Message.Subject.Should().Be("Subject");
            smtpClient.Message.From.Single().ToString().Should().Be("sender@notino.com");
        }

        [Test]
        public async Task ThrowExampleWebApplicationFactory()
        {
            var webApp = new WebApp();
            var client = webApp.CreateClient();

            var registerUser = await client.GetAsync("");

            registerUser.IsSuccessStatusCode.Should().BeTrue();
        }


        [Test]
        public async Task ThrowExampleRidge()
        {
            var webApp = new WebApp();
            var client = webApp.CreateClient();
            var controllerFactory = new ControllerFactory(client, webApp.Services, new NunitLogWriter());
            var sut = controllerFactory.CreateController<UserController>();

            var result = sut.Throw();

            result.IsSuccessStatusCode().Should().BeTrue();
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
