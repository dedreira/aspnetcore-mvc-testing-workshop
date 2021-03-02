using Moq;
using MvcMovie.Models;
using MvcMovie.Repositories;
using MvcMovie.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovie.Tests.UnitTests
{
    public class MoviesServiceTests
    {

        [Fact]
        public async Task Should_Call_Repository_On_CreateMovieAsync()
        {
            // Arrange
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);

            // Act
            await systemUnderTest.CreateMovieAsync(new Models.Movie());

            // Assert
            mockMovieRepository.Verify(x => x.CreateMovieAsync(It.IsAny<Movie>()), Times.Once());
        }

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

        [Fact]
        public async Task Should_Throw_MovieNotFoundException_On_GetMovie_When_Id_Is_Null()
        {
            // Arrange
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);            
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);

            // Act
            Task getMovie = systemUnderTest.GetMovie(null);

            // Assert
            await Assert.ThrowsAsync<MovieNotFoundException>(() =>  getMovie);

        }

        [Fact]
        public async Task Should_Return_Movie_On_GetMovie_When_Repository_Found_It()
        {
            // Arrange
            int id = 1;
            Movie expected = new Movie();
            Mock<IMoviesRepository> mockMovieRepository = new Mock<IMoviesRepository>(MockBehavior.Loose);
            mockMovieRepository.Setup(x => x.GetByIdAsync(id)).Returns(Task.FromResult(expected));
            MoviesService systemUnderTest = new MoviesService(mockMovieRepository.Object);

            // Act
            Movie returnValue =await systemUnderTest.GetMovie(id);

            // Assert
            Assert.Equal(expected, returnValue);
        }

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
    }
}
