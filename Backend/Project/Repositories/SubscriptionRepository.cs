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

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
    }
    

    public async Task DeleteSubscriptionAsync(int subscriptionId)
    {
        var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
        if (subscription == null) throw new SubscriptionsNotFoundException(subscriptionId);

        _context.Subscriptions.Remove(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task<Subscription?> GetSubscriptionByIdAsync(int subscriptionId)
    {
        return await _context.Subscriptions
            .Include(s => s.Confirmations)
            .Include(s => s.StreamingService)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
    {
        return await _context.Subscriptions
            .Include(s => s.Confirmations)
            .Include(s => s.StreamingService)
            .ToListAsync();
    }

    public async Task<SubscriptionConfirmation?> GetLatestConfirmationForUserAsync(int userId, int subscriptionId)
    {
        return await _context.SubscriptionConfirmations
            .Include(sc => sc.User)
            .Include(sc => sc.Subscription)
            .ThenInclude(s => s.StreamingService)
            .Where(sc => sc.UserId == userId && sc.SubscriptionId == subscriptionId)
            .OrderByDescending(sc => sc.StartTime)
            .FirstOrDefaultAsync();
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

    public async Task<List<SubscriptionConfirmation>> GetConfirmationDetailsOfSubscription(Subscription subscription)
    {
        return await _context.SubscriptionConfirmations
            .Include(sc => sc.User)
            .Include(sc => sc.Subscription)
            .Where(sc => sc.SubscriptionId == subscription.Id)
            .OrderBy(sc => sc.StartTime)
            .ToListAsync();
    }

}