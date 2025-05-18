using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Models;

namespace Project.Repositories;

public class SubscriptionConfirmationRepository
{
    private readonly MasterContext _context;

    public SubscriptionConfirmationRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

    public async Task AddSubscriptionConfirmationAsync(SubscriptionConfirmation subscriptionConfirmation)
    {
        await _context.SubscriptionConfirmations.AddAsync(subscriptionConfirmation);
    }

    public async Task<bool> HasUserConfirmedSubscriptionAsync(int userId, int streamingServiceId)
    {
        return await _context.SubscriptionConfirmations
            .AnyAsync(c => c.UserId == userId && c.Subscription.StreamingServiceId == streamingServiceId);
    }
}