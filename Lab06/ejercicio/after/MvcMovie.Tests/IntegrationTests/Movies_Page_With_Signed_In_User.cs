using MvcMovie.Tests.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovie.Tests.IntegrationTests
{
    [Collection("Secuential Integration Tests")]
    public class Movies_Page_With_Signed_In_User : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private HttpClient client;

        public Movies_Page_With_Signed_In_User(CustomWebApplicationFactory<Startup> _factory)
        {
            factory = _factory;
            client = factory
                .WithMoviesInDatabase(MoviesCatalog.GetMovies())
                .WithUserLoggedIn(
                        new MockIdentityBuilder()
                        .WithName("Test User")
                        .Identity)
                .CreateClient();
        }

        [Fact]
        public async Task Should_Return_Access_Denied_When_Try_To_Visit_Create_Movie_Page()
        {
            // Act
            var response = await client.GetAsync("movies/create");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Contains("Identity/Account/AccessDenied", response.RequestMessage.RequestUri.LocalPath);
        }

        [Fact]
        public async Task Should_Return_Access_Denied_When_Try_To_Visit_Update_Movie_Page()
        {
            // Arrange
            int filmId = MoviesCatalog.GetMovies().First().Id;

            // Act
            var response = await client.GetAsync($"movies/edit/{filmId}");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Contains("Identity/Account/AccessDenied", response.RequestMessage.RequestUri.LocalPath);
        }
        [Fact]
        public async Task Should_Return_Access_Denied_When_Try_To_Visit_Delete_Movie_Page()
        {
            // Arrange
            int filmId = MoviesCatalog.GetMovies().First().Id;

            // Act
            var response = await client.GetAsync($"movies/delete/{filmId}");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Contains("Identity/Account/AccessDenied", response.RequestMessage.RequestUri.LocalPath);
        }
    }

}
