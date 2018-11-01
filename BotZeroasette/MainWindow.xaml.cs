using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace BotZeroasette
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentExeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var localita = TextBox1.Text;

            using (var chromeDriver = new ChromeDriver(currentExeDirectory))
            {
                chromeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

                var explicitWait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(3));

                chromeDriver.Navigate().GoToUrl("https://www.ilmeteo.it/");
                chromeDriver.Manage().Window.Maximize();

                var searchElement = chromeDriver.FindElement(By.Id("search-main"));
                searchElement.SendKeys(localita);

                var options = explicitWait.Until(driver => driver.FindElement(By.Id("ajax_listOfOptions")));

                var firstOptionElement = explicitWait.Until(driver => options.FindElement(By.CssSelector("div:first-child > b:first-child")));
                var l2 = firstOptionElement.Text;
                firstOptionElement.Click();

                var dayTabsElement = explicitWait.Until(driver => driver.FindElement(By.Id("daytabs")));
                var tomorrowElementClickable = dayTabsElement.FindElement(By.CssSelector("li:nth-of-type(3) > a:first-child"));
                var a = tomorrowElementClickable.FindElement(By.ClassName("tmax")).Text;
                var b = tomorrowElementClickable.FindElement(By.ClassName("tmin")).Text;
                tomorrowElementClickable.Click();
                
                var tomorrow = DateTime.Now.AddDays(1);
                var elementName = $"#h13-{tomorrow.Day}";

                var frame = chromeDriver.SwitchTo().Frame("frmprevi");
                
                var row13 = explicitWait.Until(driver => frame.FindElement(By.CssSelector(elementName)));
                var c = row13.FindElement(By.CssSelector("td:nth-of-type(3)")).Text;
                var d = row13.FindElement(By.CssSelector("td:nth-of-type(4)")).Text;
                
                var message = $"Domani {tomorrow.ToShortDateString()} alle ore 13:00, nella località di {l2} il tempo sarà {c} con una temperatura di {d}C. Durante la giornata di domani le temperature oscilleranno tra {b}C e {a}C.";

                TextBox2.Text = message;
            }
        }
    }
}
