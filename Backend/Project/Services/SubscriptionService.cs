using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs.SubscriptionDTOs;
using Project.Exceptions;
using Project.Helper;
using Project.Models;
using Project.Repositories;

namespace Project.Services;

public class SubscriptionService
{
    
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly MediaContentRepository _mediaContentRepository;
    private readonly UserRepository _userRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;
    private readonly StreamingServiceRepository _streamingServiceRepository;
    private readonly SubscriptionConfirmationRepository _subscriptionConfirmationRepository;
    private readonly MasterContext _context;
    public SubscriptionService(
        MasterContext context,
        MediaContentRepository mediaContentRepository,
        UserRepository userRepository,
        SubscriptionRepository subscriptionRepository,
        WatchHistoryRepository watchHistoryRepository,
        StreamingServiceRepository streamingServiceRepository,
        SubscriptionConfirmationRepository subscriptionConfirmationRepository)
    {
        _context = context;
        _mediaContentRepository = mediaContentRepository;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
        _watchHistoryRepository = watchHistoryRepository;
        _streamingServiceRepository = streamingServiceRepository;
        _subscriptionConfirmationRepository = subscriptionConfirmationRepository;
    }
    
    public async Task AddSubscriptionAsync(AddSubscriptionDTO s)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var streamingService = await _streamingServiceRepository.GetStreamingServiceById(s.StreamingServiceId);
            if (streamingService == null)
                throw new StreamingServiceNotFoundException(s.StreamingServiceId);

            var user = await _userRepository.GetUserWithIdAsync(s.UserId);
            if (user == null)
                throw new UserNotFoundException(s.UserId);


            if (await _subscriptionConfirmationRepository.HasUserConfirmedSubscriptionAsync(user.Id,s.StreamingServiceId))
                throw new SubscriptionAlreadyExistsException(user.Id,streamingService.Id);

            
            
            var subscription = new Subscription
            {
                StreamingServiceId = s.StreamingServiceId,
                StreamingService = streamingService
            };
            await _subscriptionRepository.AddSubscriptionAsync(subscription);

            var today = DateTime.Today;
            var confirmation = new SubscriptionConfirmation
            {
                StartTime = today,
                EndTime = today.AddMonths(s.DurationInMonth),
                PaymentMethod = s.PaymentMethod,
                Price = SubscriptionPriceCalculator.CalculateAmount(streamingService.DefaultPrice, user),
                Subscription = subscription,
                SubscriptionId = subscription.Id,
                User = user,
                UserId = user.Id
            };
            await _subscriptionConfirmationRepository.AddSubscriptionConfirmationAsync(confirmation);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }



    
    public async Task DeleteSubscriptionAsync(int subscriptionId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var existing = await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
            if (existing == null)
                throw new SubscriptionsNotFoundException(subscriptionId);

            await _subscriptionRepository.DeleteSubscriptionAsync(subscriptionId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
        
        
    }

    public async Task<SubscriptionResponseDTO> GetSubscriptionByIdAsync(int subscriptionId)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
        if (subscription == null) throw new SubscriptionsNotFoundException(subscriptionId);
        return new SubscriptionResponseDTO()
        {
            Id = subscription.Id,
            DaysLeft = subscription.DurationInDays,
            Price = subscription.StreamingService.DefaultPrice,
            StreamingServiceName = subscription.StreamingService.Name
        };
    }




    public async Task<IEnumerable<SubscriptionResponseDTO>> GetAllSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();
        
        var result = subscriptions
            .Select(s => new SubscriptionResponseDTO()
            {
                Id = s.Id,
                DaysLeft = s.DurationInDays, 
                Price = s.StreamingService.DefaultPrice, 
                StreamingServiceName = s.StreamingService.Name
            });

        return result;

    }

    public async Task<IEnumerable<SubscriptionResponseDTO>> GetActiveSubscriptionsOfUserIdAsync(int userId)
    {
        var confirmations = await _subscriptionRepository.GetUserSubscriptionConfirmationsAsync(userId);

        var activeConfirmations = confirmations
            .GroupBy(c => c.SubscriptionId)
            .Select(g => g.OrderByDescending(c => c.StartTime).First())
            .Where(latest => latest.EndTime > DateTime.UtcNow);

        var result = activeConfirmations.Select(c => new SubscriptionResponseDTO
        {
            Id = c.Id,
            DaysLeft = c.Subscription.DurationInDays,
            StreamingServiceName = c.Subscription.StreamingService.Name,
            Price = c.Price
        });

        return result;
    }


    public async Task<SubscriptionConfirmationResponseDTO?> GetLatestConfirmationForUserAsync(int userId,
        int subscriptionId)
    {
        var confirmation = await _subscriptionRepository.GetLatestConfirmationForUserAsync(userId, subscriptionId);

        if (confirmation == null)
            throw new SubscriptionConfirmationNotFoundException(subscriptionId);

        var today = DateTime.Today;
        return new SubscriptionConfirmationResponseDTO
        {
            Id = confirmation.Id,
            PaymentMethod = confirmation.PaymentMethod,
            Price = confirmation.Price,
            DurationInDays = (confirmation.EndTime - confirmation.StartTime).Days,
            UserId = confirmation.UserId,
            SubscriptionId = confirmation.SubscriptionId,
            UserStatus = confirmation.User.Status.ToString(),
            UserCountry = confirmation.User.Country,
            StreamingServiceName = confirmation.Subscription.StreamingService.Name
        };
    }


    public async Task<IEnumerable<SubscriptionConfirmationResponseDTO?>> GetConfirmationDetailsOfSubscriptionAsync(
        Subscription subscription)
    {
        var confirmations = await _subscriptionRepository.GetConfirmationDetailsOfSubscription(subscription);

        if (confirmations == null)
            throw new SubscriptionConfirmationNotFoundException(subscription.Id);
        var today = DateTime.Today;

        return confirmations.Select(confirmation => new SubscriptionConfirmationResponseDTO()
        {
            Id = confirmation.Id,
            PaymentMethod = confirmation.PaymentMethod,
            Price = confirmation.Price,
            DurationInDays = (confirmation.EndTime - confirmation.StartTime).Days,
            UserId = confirmation.UserId,
            SubscriptionId = confirmation.SubscriptionId,
            UserStatus = confirmation.User.Status.ToString(),
            UserCountry = confirmation.User.Country,
            StreamingServiceName = confirmation.Subscription.StreamingService.Name
        });
    }
}