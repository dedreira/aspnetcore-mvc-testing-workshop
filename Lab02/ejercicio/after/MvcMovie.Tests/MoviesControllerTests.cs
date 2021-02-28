using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MvcMovie.Controllers;
using MvcMovie.Models;
using MvcMovie.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovie.Tests
{
    public class MoviesControllerTests
    {      
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
    }
}
