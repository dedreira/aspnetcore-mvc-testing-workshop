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

# Paso 7. Crear un presentador para la página de Movies Index

````csharp
        [Fact]
        public void Should_Display_Movies_List_In_Movies_Page()
        {
            // Arrange
            var movies = GetMovies();

            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/Index");
            var indexMoviePage = new IndexMoviePage(Browser);
            var movieTitlesRendered = indexMoviePage.MoviesRendered.Select(m => m.Title);

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
            var indexMoviePage = new IndexMoviePage(Browser);
            indexMoviePage.SelectGenre(firstGenre);

            // Act
            indexMoviePage.SendSearchRequest();
            indexMoviePage = new IndexMoviePage(Browser);

            var genresdisplayed = indexMoviePage.MoviesRendered.Select(m => m.Title);

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
            var indexMoviePage = new IndexMoviePage(Browser);
            indexMoviePage.FilterTitle = firstTitle;

            // Act

            indexMoviePage.SendSearchRequest();
            indexMoviePage = new IndexMoviePage(Browser);
            var elements = indexMoviePage.MoviesRendered;

            // Assert
            Assert.Equal(justOneFilm, elements.Count);
        }
````