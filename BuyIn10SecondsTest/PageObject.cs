using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BuyIn10SecondsTest
{
    public class SearchPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public SearchPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        private string inStockFilterButtonBy = "//label[@class = 'ec-form-choice-group__item'][.//div[text() = 'В наличии']]";
        private string InStockFilterAmountBy = "//label[@class = 'ec-form-choice-group__item'][.//div[text() = 'В наличии']]//div[@class = 'ec-filter__items-count ec-text-muted']";
        private string displayedProductsBy = "//div[contains(@class, 'grid-product grid-product')]";
        private string soldLabelBy = "//div[text() = 'Распродано']";
        private string minPriceInputBy = "//input[contains(@aria-label, 'от')]";
        private string maxPriceInputBy = "//input[contains(@aria-label, 'до')]";
        private string pricesLabelsBy = "//div[@class = 'grid-product__price-value ec-price-item']";

        public IWebElement inStockFilterButton => driver.FindElement(By.XPath(inStockFilterButtonBy));
        public IWebElement inStockFilterAmount => driver.FindElement(By.XPath(InStockFilterAmountBy));
        public IWebElement displayedProducts => driver.FindElement(By.XPath(displayedProductsBy));
        public IWebElement soldLabel => driver.FindElement(By.XPath(soldLabelBy));
        public IWebElement minPriceInput => driver.FindElement(By.XPath(minPriceInputBy));
        public IWebElement maxPriceInput => driver.FindElement(By.XPath(maxPriceInputBy));

        public int GetExpectedInStockAmount() => Int32.Parse(inStockFilterAmount.Text);
        public int GetVisibleInStockAmount() => driver.FindElements(By.XPath(displayedProductsBy)).Count;
        public bool IsSoldProductDisplayed()
        {
            try
            {
                return soldLabel.Displayed;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        public void WaitForWebsiteToLoad() => wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(inStockFilterButtonBy)));

        public void ClickInStockFilter()
        {
            inStockFilterButton.Click();
            Thread.Sleep(300);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(displayedProductsBy)));
        }

        public void InputPriceRange(int minPrice, int maxPrice)
        {
            minPriceInput.SendKeys(minPrice.ToString());
            maxPriceInput.SendKeys(maxPrice.ToString());
            maxPriceInput.SendKeys(Keys.Enter);
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(displayedProductsBy)));
        }

        public List<int> GetListOfDisplayedPrices()
        {
            var pricesLabels = driver.FindElements(By.XPath(pricesLabelsBy));
            string temp = new string("");
            List<int> result = new List<int>();
            foreach (var e in pricesLabels)
            {
                temp = e.Text;
                temp = temp.Substring(1, temp.IndexOf(".") - 1);
                result.Add(Int32.Parse(temp));
            }
            return result;
        }
    }
}
