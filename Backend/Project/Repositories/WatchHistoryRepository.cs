using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Models;

namespace Project.Repositories;

public class WatchHistoryRepository
{
    private readonly MasterContext _context;

    public WatchHistoryRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

    public async Task<WatchHistory?> GetWatchHistoryOfUserToGivenMediaIdAsync(int userId, int mediaId)
    {
        return await _context.WatchHistories
            .Include(w => w.MediaContent)
            .Include(w => w.User)
            .FirstOrDefaultAsync(w => w.UserId == userId && w.MediaId == mediaId);
    }

    public async Task<IEnumerable<WatchHistory>> GetWatchHistoriesOfUser(int userId)
    {
        return await _context.WatchHistories
            .Include(w => w.MediaContent)
            .Include(w => w.User)
            .Where(w => w.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<WatchHistory>> GetAllWatchHistories()
    {
        return await _context.WatchHistories
            .Include(w => w.MediaContent)
            .Include(w => w.User)
            .ToListAsync();
    }
}