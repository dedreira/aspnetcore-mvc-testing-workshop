﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using MvcMovie.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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

        private IAuthenticationService authService;
        public CustomWebApplicationFactory<TStartup> WithUserLoggedIn(ClaimsIdentity identity)
        {
            authService = new MockAuthenticationService(identity);
            return this;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RegisterAndFeedApplicationDatabase(services);
                RegisterAndFeedIdentityDatabase(services);

                if(authService != null)
                {
                    services.AddSingleton<IAuthenticationService>(authService);
                }

                services.AddAntiforgery(t =>
                {
                    t.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
                    t.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
                });
            });
        }

        private void RegisterAndFeedApplicationDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<MvcMovieContext>));

            services.Remove(descriptor);

            services.AddDbContext<MvcMovieContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
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

                if (movies != null && movies.Any())
                {
                    db.Movie.AddRange(movies);
                    db.SaveChanges();
                }
            }
        }

        private void RegisterAndFeedIdentityDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<MvcMovieIdentityContext>));

            services.Remove(descriptor);

            services.AddDbContext<MvcMovieIdentityContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryIdentityDbForTesting");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MvcMovieIdentityContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();               
            }
        }
    }
}
