# Soluciones a los ejercicios del Lab 3

## Paso 2. Crear un test para el método Create

````csharp
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
````


   
# Escribir un test para la edición de una película

El ejercicio trataba sobre crear un test de integración para el método edit, confirmando que puedes editar el título de una película correctamente.

Si te atascas o te encuentras algún problema, aquí tienes una posible solución:


````csharp
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
````









