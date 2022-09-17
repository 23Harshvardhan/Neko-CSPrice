using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Windows;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Neko_CSPrice
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtGunName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGunName.Clear();
        }

        private void txtSkinName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSkinName.Clear();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService("Redist");
            service.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");
            IWebDriver steamDriver = new ChromeDriver(service, options);

            steamDriver.Navigate().GoToUrl("https://www.steamcommunity.com/market/");
            steamDriver.FindElement(By.Id("findItemsSearchBox")).SendKeys(txtGunName.Text + " | " + txtSkinName.Text + " (" + gunQuality.Text + ")");
            steamDriver.FindElement(By.Id("findItemsSearchSubmit")).Click();
            IWebElement result = steamDriver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[2]/div[1]/div[4]/div[2]/div[2]/div/div[1]/a[1]/div/div[1]/div[2]/span[1]/span[1]"));
            valSteamPrice.Content = result.Text;
            steamDriver.Close();
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }

            IWebDriver buffDriver = new ChromeDriver(service, options);
            buffDriver.Navigate().GoToUrl("https://www.bitskins.com/");
            buffDriver.FindElement(By.Id("market_hash_name")).SendKeys(txtGunName.Text + " | " + txtSkinName.Text + " (" + gunQuality.Text + ")");
            buffDriver.FindElement(By.XPath("/html/body/div[5]/div/div[1]/div/div/div/form/div[1]/button[1]")).Click();
            IWebElement bitResult = buffDriver.FindElement(By.XPath("/html/body/div[5]/div/div[2]/div/div/div[1]/div/div[3]/h5/span"));
            valBitSkins.Content = bitResult.Text;
            buffDriver.Close();
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }

            string withTax = valSteamPrice.Content.ToString();
            string cleanText = withTax.Replace("$", "");
            string final = cleanText.Replace(" USD", "");
            double val = Convert.ToDouble(final);
            double perc = 0.05 * val;
            double ffinal = val - perc;
            notax.Content = "$" + ffinal + " USD";
        }
    }
}
