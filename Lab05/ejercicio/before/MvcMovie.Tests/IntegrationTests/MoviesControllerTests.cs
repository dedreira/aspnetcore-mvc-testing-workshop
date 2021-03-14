using Microsoft.AspNetCore.Mvc;
using MvcMovie.Controllers;
using MvcMovie.Models;
using MvcMovie.Repositories;
using MvcMovie.Services;
using MvcMovie.Tests.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovie.Tests.IntegrationTests
{
    public class MoviesControllerTests
    {
        const int ZERO_MOVIES = 0;        
        [Fact]
        public async Task Should_Return_No_Movies_On_Index_Method_When_No_Movies_In_Database()
        {
            // Arrange
            MvcMovieContext context = new MvcMovieContextBuilder()
                                        .WithInMemoryProvider()                                       
                                        .Context;
            MoviesRepository repository = new MoviesRepository(context);
            MoviesService service = new MoviesService(repository);
            MoviesController systemUnderTest = new MoviesController(service);

            // Act
            IActionResult result = await systemUnderTest.Index(string.Empty, string.Empty);

            // Assert
            Assert.True(result is ViewResult);
            Assert.True(((ViewResult)result).Model is MovieGenreViewModel);
            MovieGenreViewModel model = ((ViewResult)result).Model as MovieGenreViewModel;
            Assert.Equal(model.Movies.Count, ZERO_MOVIES);            
        }

        [Fact]
        public async Task Should_Create_Movie_In_Database_On_Create()
        {
            // Arrange
            using MvcMovieContext context = new MvcMovieContextBuilder()
                                        .WithInMemoryProvider()
                                        .Context;
            MoviesRepository repository = new MoviesRepository(context);
            MoviesService service = new MoviesService(repository);
            MoviesController systemUnderTest = new MoviesController(service);
            Movie newMutantsMovie = new Movie() { Genre = "Action", Price = 10, ReleaseDate = DateTime.Now, Title = "New Mutants" };
            // Act 
            await systemUnderTest.Create(newMutantsMovie);
            Movie movieInDatabase = context.Movie.FirstOrDefault();
            // Assert
            Assert.NotNull(movieInDatabase);
            Assert.Equal(newMutantsMovie.Title, movieInDatabase.Title);
        }
       
    }
}
