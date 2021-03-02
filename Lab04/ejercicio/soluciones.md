# Paso 5. Crear un test para la creación de una película

````csharp
 [Fact]
        public void Should_Create_New_Movie()
        {
            // Arrange            
            
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/Create");
            IWebElement titleInput = Browser.FindElement(By.Id(nameof(Movie.Title)));
            IWebElement releaseDateInput = Browser.FindElement(By.Id(nameof(Movie.ReleaseDate)));
            IWebElement genreInput = Browser.FindElement(By.Id(nameof(Movie.Genre)));
            IWebElement priceInput = Browser.FindElement(By.Id(nameof(Movie.Price)));
            IWebElement createButton = Browser.FindElement(By.CssSelector("input[type=submit]"));
            titleInput.Clear();
            genreInput.Clear();
            priceInput.Clear();
            releaseDateInput.Clear();
            
            // Act
            
            titleInput.SendKeys("Terminator 2");
            releaseDateInput.SendKeys(DateTime.Now.AddYears(-10).ToShortDateString());
            genreInput.SendKeys("Action");
            priceInput.SendKeys(10.ToString());
            createButton.Click();

            var movieTitlesRendered = Browser.FindElements(By.CssSelector("tbody > tr > td:nth-of-type(1)"))
                            .Select(x => x.Text.Trim());

            // Assert

            Assert.Contains("Terminator 2", movieTitlesRendered);
        }
````