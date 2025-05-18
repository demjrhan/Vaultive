using Project.DTOs;
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

    public async Task<SubscriptionResponseDTO> GetSubscriptionByIdAsync(int subscriptionId)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
        if (subscription == null) throw new SubscriptionsNotFoundException(subscriptionId);
        return new SubscriptionResponseDTO()
        {
            DaysLeft = subscription.DurationInDays,
            Price = subscription.DefaultPrice,
            StreamingServiceName = subscription.StreamingService.Name
        };
    }




    public async Task<IEnumerable<SubscriptionResponseDTO>> GetAllSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();
        
        var result = subscriptions
            .Select(s => new SubscriptionResponseDTO()
            {
                DaysLeft = s.DurationInDays, 
                Price = s.DefaultPrice, 
                StreamingServiceName = s.StreamingService.Name
            });

        return result;

    }

    public async Task<IEnumerable<SubscriptionResponseDTO>> GetActiveSubscriptionsOfUserIdAsync(int userId)
    {
        var subscriptions = await _subscriptionRepository.GetActiveSubscriptionsOfUserIdAsync(userId);
        var result = subscriptions.Select(s => new SubscriptionResponseDTO()
        {
            DaysLeft = s.DurationInDays,
            StreamingServiceName = s.StreamingService.Name,
            Price = s.Confirmations.FirstOrDefault().Price
            
        });

        

        return result;
    }


    public async Task<SubscriptionConfirmationResponseDTO?> GetLatestConfirmationForUserAsync(int userId,
        int subscriptionId)
    {
        var confirmation = await _subscriptionRepository.GetLatestConfirmationForUserAsync(userId, subscriptionId);

        if (confirmation == null)
            throw new SubscriptionConfirmationNotFoundException(subscriptionId);

        return new SubscriptionConfirmationResponseDTO
        {
            Id = confirmation.Id,
            PaymentMethod = confirmation.PaymentMethod,
            Price = confirmation.Price,
            StartTime = confirmation.StartTime,
            EndTime = confirmation.EndTime,
            UserId = confirmation.UserId,
            SubscriptionId = confirmation.SubscriptionId,
            UserStatus = confirmation.User.Status.ToString(),
            UserCountry = confirmation.User.Country,
            StreamingServiceName = confirmation.Subscription.StreamingService.Name
        };
    }


    public async Task<SubscriptionConfirmationResponseDTO?> GetConfirmationDetailsOfSubscriptionAsync(
        Subscription subscription)
    {
        var confirmation = await _subscriptionRepository.GetConfirmationDetailsOfSubscription(subscription);

        if (confirmation == null)
            throw new SubscriptionConfirmationNotFoundException(subscription.Id);

        return new SubscriptionConfirmationResponseDTO
        {
            Id = confirmation.Id,
            PaymentMethod = confirmation.PaymentMethod,
            Price = confirmation.Price,
            StartTime = confirmation.StartTime,
            EndTime = confirmation.EndTime,
            UserId = confirmation.UserId,
            SubscriptionId = confirmation.SubscriptionId,
            UserStatus = confirmation.User.Status.ToString(),
            UserCountry = confirmation.User.Country,
            StreamingServiceName = confirmation.Subscription.StreamingService.Name
        };
    }
}