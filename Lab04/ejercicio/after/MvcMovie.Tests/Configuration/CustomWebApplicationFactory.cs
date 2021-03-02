using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace MvcMovie.Tests.Configuration
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private List<Movie> movies;        
        public CustomWebApplicationFactory<TStartup> WithMoviesInDatabase(List<Movie> _movies)
        {
            if (movies == null)
                movies = new List<Movie>();

            movies.AddRange(_movies);
            return this;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<MvcMovieContext>));

                services.Remove(descriptor);

                services.AddDbContext<MvcMovieContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                services.AddAntiforgery(t =>
                {
                    t.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
                    t.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MvcMovieContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                    
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();   
                    
                    if(movies != null && movies.Any())
                    {
                        db.Movie.AddRange(movies);
                        db.SaveChanges();
                    }
                }
            });
        }
    }
}
