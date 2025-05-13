using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Modules;

namespace Project.Repositories;

public class SubscriptionConfirmationRepository
{
    private readonly MasterContext _context;

    public SubscriptionConfirmationRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

    public async Task<IEnumerable<SubscriptionConfirmation>?> GetUsersSubscriptionConfirmationsAsync(int userId)
    {
        return await _context.SubscriptionConfirmations
            .Where(sc => sc.UserId == userId)
            .Include(sc => sc.Subscription)
            .ThenInclude(s => s.StreamingService)
            .ToListAsync();
    }
}