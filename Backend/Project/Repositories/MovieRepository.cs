using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Models;
using Project.Models.Enumerations;

namespace Project.Repositories;

public class MovieRepository
{
    private readonly MasterContext _context;

    public MovieRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

    
    public async Task<IEnumerable<Movie>> GetMoviesWithGivenGenre(Genre genre)
    {
        return (await _context.Movies.ToListAsync())
            .Where(m => m.Genres.Contains(genre));
    }

    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        return await _context.Movies.ToListAsync();
    }
}