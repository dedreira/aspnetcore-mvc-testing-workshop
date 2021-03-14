using MvcMovie.Tests.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
namespace MvcMovie.Tests.IntegrationTests
{
    [Collection("Secuential Integration Tests")]
    public class Movies_Page_With_Anonymous_User : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private HttpClient client;
        public Movies_Page_With_Anonymous_User(CustomWebApplicationFactory<Startup> _factory)
        {
            factory = _factory;
            client = factory.CreateClient();
        }

        [Fact]
        public async Task Should_Redirect_To_Login_When_Navigate_To_Index()
        {
            // Arrange

            // Act
            var response = await client.GetAsync("movies");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Contains("Identity/Account/Login", response.RequestMessage.RequestUri.LocalPath);
        }
    }
}
