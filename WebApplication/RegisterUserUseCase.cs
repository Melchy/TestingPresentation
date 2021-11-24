namespace WebApplication
{
    public class RegisterUserUseCase
    {
        private readonly EmailService _emailService;
        private readonly UserRepository _userRepository;

        public RegisterUserUseCase(
            EmailService emailService,
            UserRepository userRepository)
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
