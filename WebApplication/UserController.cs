using Microsoft.AspNetCore.Mvc;

namespace WebApplication
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly RegisterUserUseCase _registerUserUseCase;

        public UserController(
            RegisterUserUseCase registerUserUseCase)
        {
            _registerUserUseCase = registerUserUseCase;
        }

        [HttpPost("")]
        public IActionResult RegisterUser(
            [FromBody] User user)
        {
            var isSuccessful = _registerUserUseCase.Register(user);
            if (isSuccessful)
            {
                return Ok();
            }

            return BadRequest();
        }
    }

    public record User(
        string Email,
        string Name);
}
