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

    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        return await _context.Movies
            .Include(m => m.StreamingServices)
            .Include(m => m.Reviews)
            .ThenInclude(r => r.User )
            .Include(m => m.WatchHistories)
            .ThenInclude(wh => wh.User )
            .ToListAsync();

    }
    
    
    public async Task AddAsync(MediaContent mediaContent)
    {
        await _context.MediaContents.AddAsync(mediaContent);
    }

    public async Task RemoveAsync(int mediaId)
    {
        var media = await GetMediaContentWithGivenId(mediaId);
        if (media != null) _context.MediaContents.Remove(media);

    }
    /* Getting the media content, private because no access needed for outside */
    private async Task<MediaContent?> GetMediaContentWithGivenId(int mediaId)
    {
        return await _context.MediaContents
            .FirstOrDefaultAsync(m => m.Id == mediaId);

    }
}