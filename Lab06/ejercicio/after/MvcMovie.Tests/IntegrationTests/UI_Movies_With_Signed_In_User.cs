using MvcMovie.Tests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovie.Tests.IntegrationTests
{
    [Collection("Secuential Integration Tests")]
    public class UI_Movies_With_Signed_In_User : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        public SeleniumServerFactory<Startup> Server;
        public IWebDriver Browser;
        public HttpClient Client;

        public UI_Movies_With_Signed_In_User(SeleniumServerFactory<Startup> server)
        {
            Server = server;
            Client = Server
                    .WithMoviesInDatabase(MoviesCatalog.GetMovies())
                    .WithUserLoggedIn(
                        new MockIdentityBuilder()
                               .WithName("Test User")
                                .Identity)
                    .CreateClient();
            var opts = new ChromeOptions();
            opts.AddArgument("--headless");
            opts.SetLoggingPreference(OpenQA.Selenium.LogType.Browser, LogLevel.All);

            var driver = new RemoteWebDriver(opts);
            Browser = driver;
        }

        [Fact]
        public async Task Should_Return_Access_Denied_When_Try_To_Create_Movie()
        {
            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/Create");

            // Assert
            Assert.Contains("Identity/Account/AccessDenied", Browser.Url);
        }

        [Fact]
        public async Task Should_Return_Access_Denied_When_Try_To_Update_Movie()
        {
            // Arrange
            int filmId = MoviesCatalog.GetMovies().First().Id;

            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/edit/{filmId}");

            // Assert
            Assert.Contains("Identity/Account/AccessDenied", Browser.Url);
        }
        [Fact]
        public async Task Should_Return_Access_Denied_When_Try_To_Delete_Movie()
        {
            // Arrange
            int filmId = MoviesCatalog.GetMovies().First().Id;

            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/delete/{filmId}");

            // Assert
            Assert.Contains("Identity/Account/AccessDenied", Browser.Url);
        }

        public void Dispose()
        {
            Browser.Dispose();
            Client.Dispose();
        }
    }
}
