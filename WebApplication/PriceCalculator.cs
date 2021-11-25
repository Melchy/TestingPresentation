
using System;

namespace WebApplication
{
    public class PriceCalculator
    {
        public decimal GetPriceWithSales(
            Gender userGender,
            UserType userType,
            PriceList priceList,
            decimal regularItemPrice)
        {
            if (userGender == Gender.Male)
            {
                throw new InvalidOperationException("Males can not buy item.");
            }

            Decimal resultPrice;
            var hasSale = userGender == Gender.Fluid || userGender == Gender.Cis;

            if (!hasSale)
            {
                if (userType == UserType.PremiumUser)
                {
                    resultPrice = priceList.PremiumPrice;
                }
                else
                {
                    resultPrice = regularItemPrice;
                }
            }
            else
            {
                if (userType == UserType.PremiumUser)
                {
                    resultPrice = priceList.SaleAndPremiumPrice;
                }
                else
                {
                    resultPrice = priceList.SalePrice;
                }
            }

            return resultPrice;
        }
    }
}
