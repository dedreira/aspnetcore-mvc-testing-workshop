using MvcMovie.Tests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovie.Tests.IntegrationTests
{
    [Collection("Secuential Integration Tests")]
    public class UI_Movies_With_Anonymous_User : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        public SeleniumServerFactory<Startup> Server;
        public IWebDriver Browser;
        public HttpClient Client;

        public UI_Movies_With_Anonymous_User(SeleniumServerFactory<Startup> server)
        {
            Server = server;
            Client = Server                   
                    .CreateClient();
            var opts = new ChromeOptions();
            opts.AddArgument("--headless");
            opts.SetLoggingPreference(OpenQA.Selenium.LogType.Browser, LogLevel.All);

            var driver = new RemoteWebDriver(opts);
            Browser = driver;
        }

        [Fact]
        public async Task Should_Redirect_To_Login_When_Navigate_To_Index()
        {
            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies");

            // Assert
            Assert.Contains("Identity/Account/Login", Browser.Url);
        }

        public void Dispose()
        {
            Browser.Dispose();
            Client.Dispose();
        }
    }
}
