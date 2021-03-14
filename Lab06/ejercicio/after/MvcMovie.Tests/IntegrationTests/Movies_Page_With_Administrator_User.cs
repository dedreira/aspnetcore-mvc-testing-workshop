using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using MvcMovie.Models;
using MvcMovie.Tests.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovie.Tests.IntegrationTests
{
    [Collection("Secuential Integration Tests")]
    public class Movies_Page_With_Administrator_User:IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private HttpClient client;
        private const int TERMINATOR_MOVIE_ID = 1;
        private const int GREMLINS_MOVIE_ID = 2;
        public Movies_Page_With_Administrator_User(CustomWebApplicationFactory<Startup> _factory)
        {
            factory = _factory;
            client = factory
                .WithMoviesInDatabase(GetMovies()) 
                .WithUserLoggedIn(
                        new MockIdentityBuilder()
                        .WithRole("Administrator")
                        .Identity)
                .CreateClient();
        }

        private List<Movie> GetMovies()
        {
            var terminatorMovie = new Movie() { Id = TERMINATOR_MOVIE_ID, Title = "Terminator 2", Genre = "Action" };
            return new List<Movie>() { terminatorMovie };
        }

        [Fact]
        public async Task Should_Call_Index_Ok()
        {
            // Arrange                        
            var expected = System.Net.HttpStatusCode.OK;

            // Act
            var response = await client.GetAsync("movies");
            response.EnsureSuccessStatusCode();
            
            // Assert
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, expected);                        
        }

        [Fact]
        public async Task Should_Return_Movies_On_Index_With_Movies_In_Database()
        {
            // Arrange               
            var expected = System.Net.HttpStatusCode.OK;
            var terminatorMovie = GetMovies().First(x => x.Id == TERMINATOR_MOVIE_ID);

            // Act
            var response = await client.GetAsync("movies");
            response.EnsureSuccessStatusCode();
            
            string content = await response.Content.ReadAsStringAsync();


            // Assert
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, expected);
            Assert.Contains(terminatorMovie.Title, content);            
        }

        [Fact]
        public async Task Should_Create_Movie_On_Create_Page()
        {
            // Arrange    
            var gremlinsMovie = new Movie() { Id= GREMLINS_MOVIE_ID, Title = "Gremlins", Genre = "Action" };

            var expected = System.Net.HttpStatusCode.OK;
            var response = await client.GetAsync("movies/create");
            response.EnsureSuccessStatusCode();
            var (fieldValue, cookieValue) = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "movies/create");
            postRequest.Headers.Add("Cookie",
                new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName,
                                    cookieValue).ToString());

            var formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, fieldValue },
                { nameof(gremlinsMovie.Title), gremlinsMovie.Title },
                { nameof(gremlinsMovie.Genre), gremlinsMovie.Genre },
                { nameof(gremlinsMovie.Price), gremlinsMovie.Price.ToString() },
                { nameof(gremlinsMovie.ReleaseDate), gremlinsMovie.ReleaseDate.ToString() },
            };
            postRequest.Content = new FormUrlEncodedContent(formModel);


            // Act
            response = await client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, expected);
            Assert.Contains(gremlinsMovie.Title, content);            
        }

        [Fact]
        public async Task Should_Update_Movie_On_Edit()
        {
            // Arrange    
            var terminatorMovie = GetMovies().First(x => x.Id == TERMINATOR_MOVIE_ID);
            client = factory
                .WithMoviesInDatabase(new List<Movie>() { terminatorMovie })                
                .CreateClient();
            terminatorMovie.Title = "Terminator 2: Judgment Day";
            var expected = System.Net.HttpStatusCode.OK;
            var response = await client.GetAsync($"movies/edit/{terminatorMovie.Id}");
            response.EnsureSuccessStatusCode();
            var (fieldValue, cookieValue) = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"movies/edit/{terminatorMovie.Id}");
            postRequest.Headers.Add("Cookie",
                new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName,
                                    cookieValue).ToString());

            var formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, fieldValue },
                { nameof(terminatorMovie.Title), terminatorMovie.Title },
                { nameof(terminatorMovie.Genre), terminatorMovie.Genre },
                { nameof(terminatorMovie.Price), terminatorMovie.Price.ToString() },
                { nameof(terminatorMovie.ReleaseDate), terminatorMovie.ReleaseDate.ToString() },
            };
            postRequest.Content = new FormUrlEncodedContent(formModel);


            // Act
            response = await client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, expected);
            Assert.Contains(terminatorMovie.Title, content);            
        }

    }
}
