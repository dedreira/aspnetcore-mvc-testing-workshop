using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Services
{
    public interface IMoviesService
    {
        IQueryable<string> GetAllGenres();
        IQueryable<Movie> GetMoviesFiltered(string movieGenre, string searchString);
        Task<Movie> GetMovie(int? id);
        Task CreateMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(int id);
    }
}
