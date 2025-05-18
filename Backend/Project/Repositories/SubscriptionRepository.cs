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

    public async Task UpdateSubscriptionAsync(Subscription updatedSubscription)
    {
        var existing = await _context.Subscriptions.FindAsync(updatedSubscription.Id);
        if (existing == null) throw new SubscriptionsNotFoundException(updatedSubscription.Id);

        existing.DefaultPrice = updatedSubscription.DefaultPrice;
        existing.StreamingServiceId = updatedSubscription.StreamingServiceId;
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


    public async Task<IEnumerable<Subscription>> GetActiveSubscriptionsOfUserIdAsync(int userId)
    {
        var subscriptions = await _context.Subscriptions
            .Include(s => s.Confirmations)
            .ThenInclude(c => c.User) 
            .Include(s => s.Confirmations)
            .Include(s => s.StreamingService)
            .Where(s => s.Confirmations.Any(c => c.UserId == userId))
            .ToListAsync();


        return subscriptions
            .Where(s =>
            {
                var latest = s.Confirmations
                    .Where(c => c.UserId == userId).MaxBy(c => c.StartTime);

                return latest != null && latest.EndTime > DateTime.UtcNow;
            });
    }


    public async Task<SubscriptionConfirmation?> GetConfirmationDetailsOfSubscription(Subscription subscription)
    {
        return await _context.SubscriptionConfirmations
            .Include(sc => sc.User)
            .Include(sc => sc.Subscription)
            .ThenInclude(s => s.StreamingService)
            .Where(sc => sc.SubscriptionId == subscription.Id)
            .OrderByDescending(sc => sc.StartTime)
            .FirstOrDefaultAsync();
    }

}