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
    
    public async Task<Movie?> GetMediaContentWithGivenId(int mediaId)
    {
        return await _context.Movies
            .Include(m => m.StreamingServices)
            .Include(m => m.Reviews)
            .ThenInclude(r => r.User)
            .Include(m => m.WatchHistories)
            .ThenInclude(wh => wh.User)
            .Where(m => m.Id == mediaId)
            .FirstOrDefaultAsync();
    }
    
    public async Task AddAsync(MediaContent mediaContent)
    {
        await _context.MediaContents.AddAsync(mediaContent);
    }

    public async Task RemoveAsync(MediaContent mediaContent)
    {
        var content = await _context.MediaContents.FindAsync(mediaContent);
        if (content == null) throw new MediaContentDoesNotExistsException(mediaContent.Id);

        _context.MediaContents.Remove(mediaContent);

    }
}