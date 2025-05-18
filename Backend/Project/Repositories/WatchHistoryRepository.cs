using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Exceptions;
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
       await _context.WatchHistories.AddAsync(watchHistory);
    }

    public async Task UpdateAsync(WatchHistory existing, WatchHistory updated)
    {
        existing.TimeLeftOf = updated.TimeLeftOf;
        existing.WatchDate = updated.WatchDate;
    }

    public async Task DeleteAsync(WatchHistory watchHistory)
    {
        var history = await _context.WatchHistories.FindAsync(watchHistory);
        if (history == null) throw new WatchHistoryNotFoundException(watchHistory.Id);

        _context.WatchHistories.Remove(history);
    }
    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new UserNotFoundException(userId);

        _context.Users.Remove(user);
    }
}