using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using OpenQA.Selenium.Support.UI;

namespace BotZeroasette
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWebDriver _chromeDriver;
        public IWebDriver ChromeDriver {
            get {
                if (_chromeDriver != null) return _chromeDriver;
                // Location of the BotZeroasette.exe 
                var currentExeDirectory = AppDomain.CurrentDomain.BaseDirectory;

                var options = new ChromeOptions
                {
                    BinaryLocation = Path.Combine(currentExeDirectory, "chromedriver.exe")
                };
                _chromeDriver = new ChromeDriver(options);

                return _chromeDriver;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentExeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var options = new ChromeOptions
            {
                BinaryLocation = Path.Combine(currentExeDirectory, "chromedriver.exe")
            };
            //options.SetLoggingPreference(LogType.Browser, LogLevel.Off);
            options.AddArgument("--log-level=ALL");
            using (var chromeDriver = new ChromeDriver(currentExeDirectory))
            {
                //Notice navigation is slightly different than the Java version
                //This is because 'get' is a keyword in C#
                chromeDriver.Navigate().GoToUrl("http://www.google.com/");

                // Find the text input element by its name
                IWebElement query = chromeDriver.FindElement(By.Name("q"));

                // Enter something to search for
                query.SendKeys("Cheese");

                // Now submit the form. WebDriver will find the form for us from the element
                query.Submit();

                // Google's search is rendered dynamically with JavaScript.
                // Wait for the page to load, timeout after 10 seconds
                var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.Title.StartsWith("cheese", StringComparison.OrdinalIgnoreCase));

                // Should see: "Cheese - Google Search" (for an English locale)
                Console.WriteLine("Page title is: " + chromeDriver.Title);
            }
        }
    }
}
