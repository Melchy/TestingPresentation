namespace WebApplication
{
    public class RegisterUserUseCase
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public RegisterUserUseCase(
            IEmailService emailService,
            IUserRepository userRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public bool Register(
            User user)
        {
            if (user.Name.StartsWith("a"))
            {
                return false;
            }

            _userRepository.SaveUser(user);
            _emailService.SendRegistrationEmail(user.Email);
            return true;
        }
    }
}
