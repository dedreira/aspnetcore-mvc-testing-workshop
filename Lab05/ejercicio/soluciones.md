# Paso 6. Crear tests para un usuario no logado en el sistema

Para la clase **Movies_Page_With_Anonymous_User**:

````csharp
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
````

Para la clase **UI_Movies_With_Anonymous_User**

````csharp
        [Fact]
        public async Task Should_Redirect_To_Login_When_Navigate_To_Index()
        {
            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies");

            // Assert
            Assert.Contains("Identity/Account/Login", Browser.Url);
        }
````