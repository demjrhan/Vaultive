using Project.Exceptions;
using Project.Models;
using Project.Repositories;

namespace Project.Services;

public class SubscriptionService
{
    private readonly SubscriptionRepository _subscriptionRepository;

    public SubscriptionService(SubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        if (subscription.DefaultPrice <= 0)
            throw new ArgumentException("Default price must be greater than 0.");

        await _subscriptionRepository.AddSubscriptionAsync(subscription);
    }

    public async Task UpdateSubscriptionAsync(Subscription updatedSubscription)
    {
        var existing = await _subscriptionRepository.GetSubscriptionByIdAsync(updatedSubscription.Id);
        if (existing == null)
            throw new SubscriptionsNotFoundException(updatedSubscription.Id);

        if (existing.DefaultPrice == updatedSubscription.DefaultPrice &&
            existing.StreamingServiceId == updatedSubscription.StreamingServiceId)
        {
            throw new NoChangesDetectedException(); 
        }

        await _subscriptionRepository.UpdateSubscriptionAsync(updatedSubscription);
    }

    public async Task DeleteSubscriptionAsync(int subscriptionId)
    {
        var existing = await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
        if (existing == null)
            throw new SubscriptionsNotFoundException(subscriptionId);

        await _subscriptionRepository.DeleteSubscriptionAsync(subscriptionId);
    }

    public async Task<Subscription?> GetSubscriptionByIdAsync(int subscriptionId)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
        if (subscription == null)
            throw new SubscriptionsNotFoundException(subscriptionId);

        return subscription;
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
    {
        return await _subscriptionRepository.GetAllSubscriptionsAsync();
    }

    public async Task<IEnumerable<Subscription>> GetActiveSubscriptionsOfUserIdAsync(int userId)
    {
        var result = await _subscriptionRepository.GetActiveSubscriptionsOfUserIdAsync(userId);
        if (!result.Any())
            throw new SubscriptionsNotFoundException(userId);

        return result;
    }

    public async Task<SubscriptionConfirmation?> GetLatestConfirmationForUserAsync(int userId, int subscriptionId)
    {
        return await _subscriptionRepository.GetLatestConfirmationForUserAsync(userId, subscriptionId);
    }

    public async Task<SubscriptionConfirmation?> GetConfirmationDetailsOfSubscriptionAsync(Subscription subscription)
    {
        return await _subscriptionRepository.GetConfirmationDetailsOfSubscription(subscription);
    }
}
