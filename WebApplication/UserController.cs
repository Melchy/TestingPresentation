using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly BuyItemUseCase _buyItemUseCase;

        public CartController(
            BuyItemUseCase buyItemUseCase)
        {
            _buyItemUseCase = buyItemUseCase;
        }

        [HttpPost("{userId:guid}/{itemId:guid}")]
        public async Task<IActionResult> RegisterUser(
            [FromRoute] Guid userId,
            [FromRoute] Guid itemId)
        {
            await _buyItemUseCase.Buy(userId, itemId);
            return Ok();
        }
    }

    public record User(
        string Email,
        string Name,
        UserType UserType,
        int UserAge,
        Gender Gender);

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Cis = 3,
        Fluid = 4,
    }

    public enum UserType
    {
        PremiumUser = 1,
        RegularUser = 2,
        CompanyClient = 3,
    }
}
