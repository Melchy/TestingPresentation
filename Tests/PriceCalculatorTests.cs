using FluentAssertions;
using NUnit.Framework;
using WebApplication;

namespace Tests
{
    public class PriceCalculatorTests
    {
        [Test]
        public void WhenUserIsCisAndPremiumUserThenSaleAndPremiumPriceIsApplied()
        {
            var priceList = new PriceList()
            {
                PremiumPrice = 100,
                SaleAndPremiumPrice = 50,
                SalePrice = 80,
            };

            var sut = new PriceCalculator();
            var priceWithSales = sut.GetPriceWithSales(Gender.Cis, UserType.PremiumUser, priceList, 150);

            priceWithSales.Should().Be(priceList.SaleAndPremiumPrice);
        }
    }
}
