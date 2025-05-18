using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Exceptions;
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
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task AddMovieAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
    }
    public async Task DeleteMovieAsync(string title)
    {
        var movie = await _context.Movies.FindAsync(title);
        if (movie == null) throw new MovieNotFoundException(title);

        _context.Movies.Remove(movie);
    }
    public async Task<Movie?> GetMovieByTitleAsync(string title)
    {
        return await _context.Movies
            .Include(m => m.MediaContentStreamingServices)
            .ThenInclude(mcs => mcs.StreamingService)
            .FirstOrDefaultAsync(m => m.Title == title);
    }

    
    public async Task<IEnumerable<Movie>> GetMoviesWithGivenGenre(Genre genre)
    {
        var movies = await _context.Movies.ToListAsync();
        return movies.Where(m => m.Genres.Contains(genre));
    }

    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        return await _context.Movies
            .Include(m => m.MediaContentStreamingServices)
            .ThenInclude(mcs => mcs.StreamingService)
            .ToListAsync();

    }
    public async Task UpdateMovieAsync(Movie updatedMovie)
    {
        var existing = await _context.Movies.FindAsync(updatedMovie.Title);
        if (existing == null) throw new MovieNotFoundException(updatedMovie.Title);

        existing.Description = updatedMovie.Description;
        existing.ReleaseDate = updatedMovie.ReleaseDate;
        existing.OriginalLanguage = updatedMovie.OriginalLanguage;
        existing.Country = updatedMovie.Country;
        existing.Duration = updatedMovie.Duration;
        existing.PosterImageName = updatedMovie.PosterImageName;
        existing.YoutubeTrailerURL = updatedMovie.YoutubeTrailerURL;
        existing.Genres = updatedMovie.Genres;
        existing.SubtitleOptionId = updatedMovie.SubtitleOptionId;
        existing.AudioOptionId = updatedMovie.AudioOptionId;
    }

}