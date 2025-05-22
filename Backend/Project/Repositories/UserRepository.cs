using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs.UserDTOs;
using Project.Exceptions;
using Project.Models;

namespace Project.Repositories;

public class UserRepository
{
    private readonly MasterContext _context;

    public UserRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }
   
    public async Task RemoveAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new UserNotFoundException(userId);
        _context.Users.Remove(user);
    }
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users

            // Reviews → MediaContent, WatchHistory
            .Include(u => u.Reviews)
            .ThenInclude(r => r.MediaContent)
            .Include(u => u.Reviews)
            .ThenInclude(r => r.WatchHistory)

            // WatchHistories → MediaContent
            .Include(u => u.WatchHistories)
            .ThenInclude(wh => wh.MediaContent)

            // Confirmations → Subscription → StreamingService
            .Include(u => u.Confirmations)
            .ThenInclude(c => c.Subscription)
            .ThenInclude(s => s.StreamingService)

            .ToListAsync();
    }

    public async Task<User?> GetUserWithGivenId(int userId)
    {
        return await _context
            .Users
            .Include(u => u.Reviews)
            .ThenInclude(r => r.MediaContent)
            .ThenInclude(m => m.WatchHistories)
            .Include(u => u.Confirmations)
            .ThenInclude(c => c.Subscription)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}