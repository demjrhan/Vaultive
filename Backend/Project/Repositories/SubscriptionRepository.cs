using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Exceptions;
using Project.Models;

namespace Project.Repositories;

public class SubscriptionRepository
{
    private readonly MasterContext _context;

    public SubscriptionRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
    {
        return await _context.Subscriptions
            .Include(s => s.Confirmations)
            .ThenInclude(c => c.User)
            .Include(s => s.StreamingService)
            .ToListAsync();
    }
    public async Task<Subscription?> GetSubscriptionWithGivenIdAsync(int subscriptionId)
    {
        return await _context.Subscriptions
            .Include(s => s.Confirmations)
            .ThenInclude(sc => sc.User)
            .Include(s => s.StreamingService)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }
    public async Task<IEnumerable<SubscriptionConfirmation>> GetUserSubscriptionConfirmationsAsync(int userId)
    {
        return await _context.SubscriptionConfirmations
            .Include(sc => sc.Subscription)
            .ThenInclude(s => s.StreamingService)
            .Include(sc => sc.User)
            .Where(sc => sc.UserId == userId)
            .ToListAsync();
    }
    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
    }
    public async Task RemoveAsync(int subscriptionId)
    {
        var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
        if (subscription == null) throw new SubscriptionsNotFoundException(subscriptionId);
        _context.Subscriptions.Remove(subscription);
    }

}