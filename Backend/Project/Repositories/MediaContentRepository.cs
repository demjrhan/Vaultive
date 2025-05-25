using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Exceptions;
using Project.Models;

namespace Project.Repositories;

public class MediaContentRepository
{
    private readonly MasterContext _context;

    public MediaContentRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        return await _context.Movies
            .Include(m => m.StreamingServices)
            .Include(m => m.Reviews)
            .ThenInclude(r => r.User)
            .Include(m => m.WatchHistories)
            .ThenInclude(wh => wh.User)
            .Include(m => m.AudioOption)
            .Include(m => m.SubtitleOption)
            .ToListAsync();
    }

    public async Task<MediaContent?> GetMediaContentWithGivenIdAsync(int mediaId)
    {
        return await _context.MediaContents
            .Include(m => m.StreamingServices)
            .Include(m => m.Reviews)
            .ThenInclude(r => r.User)
            .Include(m => m.WatchHistories)
            .ThenInclude(wh => wh.User)
            .Include(m => m.AudioOption)
            .Include(m => m.SubtitleOption)
            .FirstOrDefaultAsync(m => m.Id == mediaId);
    }

    /* Used for updating the media content */
    public async Task<Movie?> GetMovieWithGivenIdAsync(int movieId)
    {
        return await _context.Movies
            .Include(m => m.StreamingServices)
            .Include(m => m.Reviews)
            .ThenInclude(r => r.User)
            .Include(m => m.WatchHistories)
            .ThenInclude(wh => wh.User)
            .Include(m => m.AudioOption)
            .Include(m => m.SubtitleOption)
            .FirstOrDefaultAsync(m => m.Id == movieId);
    }

    public async Task AddAsync(MediaContent mediaContent)
    {
        await _context.MediaContents.AddAsync(mediaContent);
    }

    public async Task RemoveAsync(int mediaId)
    {
        var media = await GetMediaContentWithGivenIdAsync(mediaId);
        if (media == null) throw new MediaContentDoesNotExistsException(new [] {mediaId});
        _context.MediaContents.Remove(media);
    }
}