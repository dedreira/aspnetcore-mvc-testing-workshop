using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using MvcMovie.Models;
using MvcMovie.Tests.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovie.Tests.IntegrationTests
{
    public class MoviesPageTests:IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        public MoviesPageTests(CustomWebApplicationFactory<Startup> _factory)
        {
            factory = _factory;
        }
        [Fact]
        public async Task Should_Call_Index_Ok()
        {
            // Arrange            
            var client = factory.CreateClient();
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
            var terminatorMovie = new Movie() { Title = "Terminator 2", Genre = "Action" };
            var movies = new List<Movie>() { terminatorMovie };
            var client = factory
                .WithMoviesInDatabase(movies)
                .CreateClient();

            var expected = System.Net.HttpStatusCode.OK;

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
            var terminatorMovie = new Movie() { Title = "Terminator 2", Genre = "Action" };           
            var client = factory                
                .CreateClient();

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

        [Fact]
        public async Task Should_Update_Movie_On_Edit()
        {
            // Arrange    
            var terminatorMovie = new Movie() { Id=1, Title = "Terminator 2", Genre = "Action" };
            var client = factory
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
