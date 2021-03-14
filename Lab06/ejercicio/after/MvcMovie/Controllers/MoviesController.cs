using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using MvcMovie.Services;

namespace MvcMovie.Controllers
{
    [Authorize(Policy ="CanViewMovies")]
    public class MoviesController : Controller
    {
        private readonly IMoviesService service;
        public MoviesController(IMoviesService _service)
        {            
            service = _service;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = service.GetAllGenres();

            var movies = service.GetMoviesFiltered(movieGenre, searchString);

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };

            return View(movieGenreVM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Movie movie;
            try
            {
                movie = await service.GetMovie(id);
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Policy = "CanCreateMovie")]
        public IActionResult Create()
        {
            return View(new Movie
            {
                Title = "Conan",
                ReleaseDate = DateTime.Now,
                Genre = "Action",
                Price = 1.99M
                //,   Rating = "R"
            }
                );
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "CanCreateMovie")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                await service.CreateMovieAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Policy = "CanEditMovie")]
        public async Task<IActionResult> Edit(int? id)
        {
            Movie movie;
            try
            {
                movie = await service.GetMovie(id);
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            return View(movie);            
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "CanEditMovie")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await service.UpdateMovieAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Policy = "CanDeleteMovie")]
        public async Task<IActionResult> Delete(int? id)
        {
            Movie movie;
            try
            {
                movie = await service.GetMovie(id);
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            return View(movie);
        }


        // POST: Movies/Delete/5
        [Authorize(Policy = "CanDeleteMovie")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.DeleteMovieAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
