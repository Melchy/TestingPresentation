using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using WebApplication;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void UserRegistrationSimple()
        {
            var emailService = A.Fake<IEmailService>();
            var userRepository = A.Fake<IUserRepository>();

            var user = new User("reciever@email.com", "name");
            var sut = new RegisterUserUseCase(emailService, userRepository);
            var registrationSuccessful = sut.Register(user);

            registrationSuccessful.Should().BeTrue();
            A.CallTo(() => userRepository.SaveUser(user)).MustHaveHappenedOnceExactly();
            A.CallTo(() => emailService.SendRegistrationEmail(user.Email)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void NewlyRegisteredUserMustNotHaveCharacter_a_AtTheBeginningOfName()
        {
            var emailService = A.Fake<IEmailService>();
            var userRepository = A.Fake<IUserRepository>();

            var user = new User("reciever@email.com", "ahmed");
            var sut = new RegisterUserUseCase(emailService, userRepository);
            var registrationSuccessful = sut.Register(user);

            registrationSuccessful.Should().BeFalse();
            A.CallTo(() => userRepository.SaveUser(A<User>._)).MustNotHaveHappened();
            A.CallTo(() => emailService.SendRegistrationEmail(A<string>._)).MustNotHaveHappened();
        }
    }
}
