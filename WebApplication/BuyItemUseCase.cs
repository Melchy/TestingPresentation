using System;
using System.Threading.Tasks;

namespace WebApplication
{
    public class BuyItemUseCase
    {
        private readonly UserRepository _userRepository;
        private readonly ItemRepository _itemRepository;
        private readonly PriceListRepository _priceListRepository;
        private readonly CheckoutService _checkoutService;
        private readonly PriceCalculator _priceCalculator;

        public BuyItemUseCase(
            UserRepository userRepository,
            ItemRepository itemRepository,
            PriceListRepository priceListRepository,
            CheckoutService checkoutService)
        {
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _priceListRepository = priceListRepository;
            _checkoutService = checkoutService;
            _priceCalculator = new PriceCalculator();
        }

        public async Task Buy(
            Guid userId,
            Guid itemId)
        {
            var user = _userRepository.GetUser(userId);
            var item = _itemRepository.GetItem();
            var priceList = _priceListRepository.GetPriceListForItem(itemId);
            var resultPrice = _priceCalculator.GetPriceWithSales(user.Gender, user.UserType, priceList, item.Price);
            await _checkoutService.CheckoutItem(item.Id, resultPrice);
        }
    }

    public class CheckoutService
    {
        public async Task CheckoutItem(
            Guid itemId,
            Decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
