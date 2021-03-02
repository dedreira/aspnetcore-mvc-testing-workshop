using MvcMovie.Models;
using MvcMovie.Tests.Configuration;
using MvcMovie.Tests.Presenters;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;

namespace MvcMovie.Tests.IntegrationTests
{
    [Collection("Secuential Integration Tests")]
    public class SeleniumTests : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        public SeleniumServerFactory<Startup> Server;
        public IWebDriver Browser;
        public HttpClient Client;        

        public SeleniumTests(SeleniumServerFactory<Startup> server)
        {
            Server = server;
            Client = Server                    
                    .WithMoviesInDatabase(GetMovies())
                    .CreateClient();            
            var opts = new ChromeOptions();
            opts.AddArgument("--headless");
            opts.SetLoggingPreference(OpenQA.Selenium.LogType.Browser, LogLevel.All);

            var driver = new RemoteWebDriver(opts);
            Browser = driver;            
        }

        private List<Movie> GetMovies()
        {
            return new List<Movie>() {
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M
                    },
                    new Movie
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M
                    },
                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Price = 9.99M
                    },
                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 3.99M
                    }};
        }

        [Fact]
        public void Should_Display_Title_In_Main_Page()
        {     
            // Act
            Browser.Navigate().GoToUrl(Server.RootUri);

            // Assert
            Assert.Equal("Home Page - Movie App", Browser.Title);            
        }

        [Fact]
        public void Should_Display_Movies_List_In_Movies_Page()
        {
            // Arrange
            var movies = GetMovies();

            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/Index");
            var movieTitlesRendered = Browser.FindElements(By.CssSelector("tbody > tr > td:nth-of-type(1)"))
                                        .Select(x => x.Text.Trim());

            // Assert            
            foreach (var movie in movies)
            {
                Assert.Contains(movie.Title.Trim(), movieTitlesRendered);
            }                        
        }

        [Fact]
        public void Should_Filter_Movies_By_Genre()
        {
            // Arrange
            var movies = GetMovies();
            var firstGenre = movies.First().Genre;
            int expected = 1;            
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/Index");
            SelectElement select = new SelectElement(Browser.FindElement(By.TagName("select")));
            IWebElement filterButton = Browser.FindElement(By.CssSelector("input[type=submit]"));

            // Act
            select.SelectByText(firstGenre);
            filterButton.Click();

            var genresdisplayed = Browser.FindElements(By.CssSelector("tbody > tr > td:nth-of-type(3)"))
                                    .Select(x => x.Text).Distinct();

            // Assert
            Assert.Equal(expected, genresdisplayed.Count());

        }

        [Fact]
        public void Should_Filter_Movies_By_Title()
        {
            // Arrange
            var movies = GetMovies();
            var firstTitle = movies.First().Title;
            int justOneFilm = 1;
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/Index");
            IWebElement filterInput = Browser.FindElement(By.CssSelector("input[type=text]"));
            IWebElement filterButton = Browser.FindElement(By.CssSelector("input[type=submit]"));

            // Act
            filterInput.SendKeys(firstTitle);
            filterButton.Click();
            var elements = Browser.FindElements(By.CssSelector("tbody > tr"));

            // Assert
            Assert.Equal(justOneFilm, elements.Count);
        }

        [Fact]
        public void Should_Create_New_Movie()
        {
            // Arrange            
            
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/Create");
            var page = new CreateMoviePage(Browser);
            page.Title = "Terminator 2";
            page.ReleaseDate = DateTime.Now.ToShortDateString();
            page.Genre = "Action";
            page.Price = 9.99M.ToString();

            // Act
            page.SendRequest();

            var movieTitlesRendered = Browser.FindElements(By.CssSelector("tbody > tr > td:nth-of-type(1)"))
                            .Select(x => x.Text.Trim());

            // Assert
            Assert.Contains("Terminator 2", movieTitlesRendered);
        }


        //[Fact]
        //public void 

        public void Dispose()
        {
            Browser.Dispose();
            Client.Dispose();            
        }
    }
}
