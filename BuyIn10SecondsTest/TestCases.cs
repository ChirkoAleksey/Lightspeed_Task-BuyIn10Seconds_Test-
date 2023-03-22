using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace BuyIn10SecondsTest
{
    public class Tests
    {
        IWebDriver driver;
        SearchPage searchPage;
        const string website = "https://buy-in-10-seconds.company.site/search";

        [SetUp]
        public void Setup()
        {
            driver = new FirefoxDriver();
            driver.Url = website;
            searchPage = new SearchPage(driver);
            searchPage.WaitForWebsiteToLoad();
        }

        [Test]
        public void CheckTwoFilters()
        {
            int minPrice = 1;
            int maxPrice = 4;

            //Act
            var expectedFilteredItems = searchPage.GetExpectedInStockAmount();
            searchPage.ClickInStockFilter();
            var visibleFilteredItems = searchPage.GetVisibleInStockAmount();

            //Assert
            expectedFilteredItems.Should().Be(visibleFilteredItems, "Number near filter button should reflect actual number of items filter applies to");
            searchPage.IsSoldProductDisplayed().Should().BeFalse("No 'Распродано' banners should be visible on the page");

            //Act
            searchPage.ClickInStockFilter();
            searchPage.InputPriceRange(minPrice, maxPrice);
            var displayedPrices = searchPage.GetListOfDisplayedPrices();

            //Assert
            displayedPrices.All(a => (a <= maxPrice) && (a >= minPrice)).Should().BeTrue("All displayed products should be in price range");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}