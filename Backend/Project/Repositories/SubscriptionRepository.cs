using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Modules;

namespace Project.Repositories;

public class SubscriptionRepository
{
    private readonly MasterContext _context;

    public SubscriptionRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

    public async Task<IEnumerable<Subscription>> GetUserActiveSubscriptionsAsync(int userId)
    {
        var subscriptions = await _context.Subscriptions
            .Include(s => s.Confirmations)
            .Include(s => s.StreamingService)
            .Where(s => s.Confirmations.Any())
            .ToListAsync();

        return subscriptions
            .Where(s =>
                s.Confirmations.MaxBy(c => c.StartTime)?.UserId == userId
            );
    }



    public async Task<SubscriptionConfirmation?> GetConfirmationDetailsForSubscription(Subscription subscription)
    {
        return await _context.SubscriptionConfirmations
            .Where(sc => sc.SubscriptionId == subscription.Id)
            .OrderByDescending(sc => sc.StartTime)
            .FirstOrDefaultAsync();
    }

}