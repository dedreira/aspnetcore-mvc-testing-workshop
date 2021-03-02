using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Repositories
{
    public class MoviesRepository: IMoviesRepository
    {
        private readonly MvcMovieContext context;
        public MoviesRepository(MvcMovieContext _context)
        {
            context = _context;
        }

        public IQueryable<string> GetAllGenres()
        {
            // Use LINQ to get list of genres.
            return from m in context.Movie
                                            orderby m.Genre
                                            select m.Genre;            
        }
        public IQueryable<Movie> GetAllMovies()
        {
            return from m in context.Movie
                         select m;            
        }
        public async Task<Movie> GetByIdAsync(int? id)
        {
            var movie = await context.Movie
                    .FirstOrDefaultAsync(m => m.Id == id);
            return movie;
        }

        public async Task CreateMovieAsync(Movie movie)
        {
            context.Add(movie);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await context.Movie.FindAsync(id);
            context.Movie.Remove(movie);
            await context.SaveChangesAsync();
        }

        public async Task<Movie> FindAsync(int? id)
        {
            return await context.Movie.FindAsync(id);
        }
        public async Task UpdateMovieAsync(Movie movie)
        {
            try
            {
                context.Update(movie);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exist(movie.Id))
                {
                    throw new MovieNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        public bool Exist(int id)
        {
            return context.Movie.Any(e => e.Id == id);
        }
    }
}
