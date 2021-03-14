using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MvcMovie.Tests.Configuration
{
    public class MvcMovieContextBuilder
    {
        private MvcMovieContext context;
        private DbContextOptionsBuilder<MvcMovieContext> optionsBuilder;
        private List<MvcMovie.Models.Movie> movies;
        public MvcMovieContextBuilder()
        {
            optionsBuilder = new DbContextOptionsBuilder<MvcMovieContext>();
            movies = new List<Movie>();
        }
        
        public MvcMovieContextBuilder WithInMemoryProvider()
        {
            optionsBuilder.UseInMemoryDatabase("TestingDB");
            return this;
        }

        public MvcMovieContextBuilder WithMoviesInDatabase(List<Movie> _movies)
        {
            movies.AddRange(_movies);
            return this;
        }

        public MvcMovieContext Context
        {
            get
            {
                context = new MvcMovieContext(optionsBuilder.Options);
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Movie.AddRange(movies);
                context.SaveChanges();
                return context;
            }
        }


    }
}
