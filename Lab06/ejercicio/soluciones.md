# Paso 3. Comprobar policies para los usuarios logados sin permisos

Clase **Movies_Page_With_Signed_In_User**:

````csharp
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
````


Clase **UI_Movies_With_Signed_In_User**

````csharp
        [Fact]
        public async Task Should_Return_Access_Denied_When_Try_To_Create_Movie()
        {
            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/Create");

            // Assert
            Assert.Contains("Identity/Account/AccessDenied", Browser.Url);
        }

        [Fact]
        public async Task Should_Return_Access_Denied_When_Try_To_Update_Movie()
        {
            // Arrange
            int filmId = MoviesCatalog.GetMovies().First().Id;

            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/edit/{filmId}");

            // Assert
            Assert.Contains("Identity/Account/AccessDenied", Browser.Url);
        }
        [Fact]
        public async Task Should_Return_Access_Denied_When_Try_To_Delete_Movie()
        {
            // Arrange
            int filmId = MoviesCatalog.GetMovies().First().Id;

            // Act
            Browser.Navigate().GoToUrl($"{Server.RootUri}/movies/delete/{filmId}");

            // Assert
            Assert.Contains("Identity/Account/AccessDenied", Browser.Url);
        }
````
