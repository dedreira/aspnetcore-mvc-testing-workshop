using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Repositories
{
    public interface IMoviesRepository
    {
        IQueryable<string> GetAllGenres();
        IQueryable<Movie> GetAllMovies();
        Task<Movie> GetByIdAsync(int? id);
        Task CreateMovieAsync(Movie movie);
        Task<Movie> FindAsync(int? id);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteAsync(int id);
        bool Exist(int id);
    }
}
