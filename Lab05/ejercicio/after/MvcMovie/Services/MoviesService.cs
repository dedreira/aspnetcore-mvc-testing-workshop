using MvcMovie.Models;
using MvcMovie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly IMoviesRepository repository;
        public MoviesService(IMoviesRepository _repository)
        {
            repository = _repository;
        }
        public async Task CreateMovieAsync(Movie movie)
        {
            await repository.CreateMovieAsync(movie);
        }

        public async Task DeleteMovieAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public IQueryable<string> GetAllGenres()
        {
            return repository.GetAllGenres();
        }

        public async Task<Movie> GetMovie(int? id)
        {
            if (id == null)
            {
                throw new MovieNotFoundException();
            }
            Movie movie = await repository.GetByIdAsync(id);
            if(movie == null)
            {
                throw new MovieNotFoundException();
            }
            return movie;
        }

        public IQueryable<Movie> GetMoviesFiltered(string movieGenre, string searchString)
        {
            var movies = repository.GetAllMovies();
            
            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            return movies;
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            await repository.UpdateMovieAsync(movie);
        }
    }
}
