# Soluciones a los ejercicios del Lab 2

## Paso 4. Crear tests para los métodos DeleteMovieAsync, GetAllGenres y UpdateMovieAsync

````csharp
        [Fact]
        public async Task Should_Call_Repository_On_DeleteMovieAsync()
        {
            // Arrange
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);

            // Act
            await systemUnderTest.DeleteMovieAsync(0);

            // Assert
            mockMovieRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void Should_Call_Repository_On_GetAllGenres()
        {
            // Arrange
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);

            // Act
            var result = systemUnderTest.GetAllGenres();

            // Assert
            mockMovieRepository.Verify(x => x.GetAllGenres(), Times.Once());
        }

        [Fact]
        public async Task Should_Call_Repository_On_UpdateAsync()
        {
            // Arrange
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);

            // Act
            await systemUnderTest.UpdateMovieAsync(new Movie());

            // Assert
            mockMovieRepository.Verify(x => x.UpdateMovieAsync(It.IsAny<Movie>()), Times.Once());
        }
````


## Paso 5.3. Comprobar que se devuelve una excepción cuando el repositorio no encuentra la película.

````csharp
        [Fact]
        public async Task Should_Throw_MovieNotFoundException_On_GetMovie_When_NoMovie_Is_Found()
        {
            // Arrange
            int id = 1;   
            Movie expected = null;         
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);
            mockMovieRepository.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(expected));
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);

            // Act
            Task getMovie = systemUnderTest.GetMovie(id);

            // Assert
            await Assert.ThrowsAsync<MovieNotFoundException>(() => getMovie);
        }
````


## Paso 6. Crear tests para el método GetMoviesFiltered

````csharp

        const string GENRE_DRAMA = "Drama";
        const string GENRE_ACTION = "Action";
        private Movie GetTerminatorMovie()
        {
            return new Movie() { Genre = GENRE_ACTION, Title = "Terminator 2" };
        }
        private Movie GetMillionDollarBabyMovie()
        {
            return new Movie() { Genre = "Drama", Title = "Million Dollar Baby" };
        }
        [Fact]
        public void Should_Filter_Movies_By_Genre()
        {
            // Arrange
            List<Movie> movies = new List<Movie>();
            movies.Add(GetTerminatorMovie());
            movies.Add(GetMillionDollarBabyMovie());
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);
            mockMovieRepository.Setup(x => x.GetAllMovies()).Returns(movies.AsQueryable());
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);
            int numberOfMoviesExpected = 1;
            // Act
            var returnValue = systemUnderTest.GetMoviesFiltered(GENRE_ACTION, string.Empty);

            // Assert
            Assert.Equal(returnValue.Count(), numberOfMoviesExpected);

        }

        [Fact]
        public void Should_Filter_Movies_By_Title()
        {
            // Arrange
            List<Movie> movies = new List<Movie>();
            movies.Add(GetTerminatorMovie());
            movies.Add(GetMillionDollarBabyMovie());
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);
            mockMovieRepository.Setup(x => x.GetAllMovies()).Returns(movies.AsQueryable());
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);
            int numberOfMoviesExpected = 1;
            // Act
            var returnValue = systemUnderTest.GetMoviesFiltered(string.Empty, GetTerminatorMovie().Title);

            // Assert
            Assert.Equal(returnValue.Count(), numberOfMoviesExpected);

        }

        [Fact]
        public void Should_Filter_Movies_By_Genre_And_Title()
        {
            // Arrange
            List<Movie> movies = new List<Movie>();
            movies.Add(GetTerminatorMovie());
            movies.Add(GetMillionDollarBabyMovie());
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);
            mockMovieRepository.Setup(x => x.GetAllMovies()).Returns(movies.AsQueryable());
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);
            int zeroMoviesExpected = 0;
            // Act
            var returnValue = systemUnderTest.GetMoviesFiltered(GENRE_DRAMA, GetTerminatorMovie().Title);

            // Assert
            Assert.Equal(returnValue.Count(), zeroMoviesExpected);
        }

````

## Paso 7.1. Crear test para el método Details

````csharp
        [Fact]
        public async Task Should_Return_View_With_Movie_On_Details_When_Movie_Is_Found()
        {
            // Arrange
            Mock<IMoviesService> mockService = new Mock<IMoviesService>();
            Movie aMovie = new Movie();
            int? id = 1;
            mockService.Setup(x => x.GetMovie(id)).Returns(Task.FromResult(aMovie));
            MoviesController systemUnderTest = new MoviesController(mockService.Object);

            // Act
            IActionResult result = await systemUnderTest.Details(id);

            // Assert
            Assert.True(result is ViewResult);
            Assert.True(((ViewResult)result).Model is Movie);

        }
        [Fact]
        public async Task Should_Return_NotFound_On_Details_When_MovieNotFoundException_Is_Thrown()
        {
            // Arrange
            Mock<IMoviesService> mockService = new Mock<IMoviesService>();            
            int? id = 1;
            mockService.Setup(x => x.GetMovie(id)).Throws(new MovieNotFoundException());
            MoviesController systemUnderTest = new MoviesController(mockService.Object);

            // Act
            IActionResult result = await systemUnderTest.Details(id);

            // Assert
            Assert.True(result is NotFoundResult);            
        }
````

## Paso 7.2. Crear Tests para el método Create (Post)

````csharp
        [Fact]
        public async Task Should_Return_View_On_Create_When_ModelState_Has_Errors()
        {
            // Arrange
            Mock<IMoviesService> mockService = new Mock<IMoviesService>();
            MoviesController systemUnderTest = new MoviesController(mockService.Object);
            systemUnderTest.ModelState.AddModelError("Error", "Unit Test");
            Movie aMovie = new Movie();
            
            // Act
            var result = await systemUnderTest.Create(aMovie);
            
            // Assert
            Assert.True(result is ViewResult);
            Assert.True(((ViewResult)result).Model is Movie);
        }

        [Fact]
        public async Task Should_Return_RedirectToAction_To_Index_On_Create_When_ModelState_Is_Ok()
        {
            // Arrange
            Mock<IMoviesService> mockService = new Mock<IMoviesService>();
            MoviesController systemUnderTest = new MoviesController(mockService.Object);            
            Movie aMovie = new Movie();
            string expectedActionName = nameof(systemUnderTest.Index);
            
            // Act
            var result = await systemUnderTest.Create(aMovie);
            
            // Assert
            Assert.True(result is RedirectToActionResult);
            Assert.Equal(((RedirectToActionResult)result).ActionName, expectedActionName);
        }
````