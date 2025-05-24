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

    
}