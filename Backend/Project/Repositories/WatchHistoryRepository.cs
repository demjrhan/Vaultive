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

    public async Task<IEnumerable<WatchHistory>> GetWatchHistoryForUsedId(int userId)
    {
        return await _context.WatchHistories.Where(w => w.UserId == userId).ToListAsync();
    }
   
}