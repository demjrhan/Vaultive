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

    public async Task<IEnumerable<WatchHistory>> GetWatchHistoriesOfUserId(int userId)
    {
        return await _context.WatchHistories.Where(w => w.UserId == userId).ToListAsync();
    }
 
    public async Task<WatchHistory?> GetByUserAndMediaAsync(int userId, string mediaTitle)
    {
        return await _context.WatchHistories
            .FirstOrDefaultAsync(w => w.UserId == userId && w.MediaTitle == mediaTitle);
    }
    public async Task AddAsync(WatchHistory watchHistory)
    {
        _context.WatchHistories.Add(watchHistory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(WatchHistory existing, WatchHistory updated)
    {
        existing.TimeLeftOf = updated.TimeLeftOf;
        existing.WatchDate = updated.WatchDate;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(WatchHistory watchHistory)
    {
        _context.WatchHistories.Remove(watchHistory);
        await _context.SaveChangesAsync();
    }
}