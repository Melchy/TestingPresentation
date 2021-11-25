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
        }

        public async Task Buy(
            Guid userId,
            Guid itemId)
        {
            var user = _userRepository.GetUser(userId);

            if (user.Gender == Gender.Male)
            {
                throw new InvalidOperationException("Males can not buy item.");
            }

            var item = _itemRepository.GetItem();
            var hasSale = user.Gender == Gender.Fluid || user.Gender == Gender.Cis;

            if (!hasSale)
            {
                if (user.UserType == UserType.PremiumUser)
                {
                    var priceList = _priceListRepository.GetPriceListForItem(itemId);
                    await _checkoutService.CheckoutItem(itemId, priceList.PremiumPrice);
                    return;
                }

                await _checkoutService.CheckoutItem(item.Id, item.Price);
            }
            else
            {
                var priceList = _priceListRepository.GetPriceListForItem(itemId);
                Decimal resultPrice;
                if (user.UserType == UserType.PremiumUser)
                {
                    resultPrice = priceList.SaleAndPremiumPrice;
                }
                else
                {
                    resultPrice = priceList.SalePrice;
                }

                await _checkoutService.CheckoutItem(itemId, resultPrice);
            }
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
