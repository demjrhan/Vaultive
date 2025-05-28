using Project.Context;
using Project.DTOs.SubscriptionDTOs;
using Project.Exceptions;
using Project.Models;
using Project.Models.Enumerations;
using Project.Repositories;

namespace Project.Services;

public class SubscriptionService
{
    private readonly ReviewRepository _reviewRepository;
    private readonly UserRepository _userRepository;
    private readonly StreamingServiceRepository _streamingServiceRepository;
    private readonly SubscriptionConfirmationRepository _subscriptionConfirmationRepository;
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly MediaContentRepository _mediaContentRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;
    private readonly MasterContext _context;

    public SubscriptionService(
        MasterContext context,
        ReviewRepository reviewRepository,
        UserRepository userRepository,
        MediaContentRepository mediaContentRepository,
        WatchHistoryRepository watchHistoryRepository,
        SubscriptionRepository subscriptionRepository,
        StreamingServiceRepository streamingServiceRepository,
        SubscriptionConfirmationRepository subscriptionConfirmationRepository)
    {
        _context = context;
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _mediaContentRepository = mediaContentRepository;
        _watchHistoryRepository = watchHistoryRepository;
        _subscriptionRepository = subscriptionRepository;
        _streamingServiceRepository = streamingServiceRepository;
        _subscriptionConfirmationRepository = subscriptionConfirmationRepository;
    }

    public async Task AddSubscriptionAsync(AddOrRenewSubscriptionDTO dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var streamingService =
                await _streamingServiceRepository.GetStreamingServiceByIdAsync(dto.StreamingServiceId);
            if (streamingService == null)
                throw new StreamingServiceDoesNotExistsException(new[] { dto.StreamingServiceId });

            var user = await _userRepository.GetUserWithGivenId(dto.UserId);
            if (user == null)
                throw new UserDoesNotExistsException(dto.UserId);

            var activeSubscriptions = await GetActiveSubscriptionsOfUserIdAsync(user.Id);
            if (activeSubscriptions.Any(ss => ss.StreamingServiceName == streamingService.Name))
                throw new SubscriptionAlreadyExistsException(user.Id, streamingService.Id);

            var today = DateOnly.FromDateTime(DateTime.Now);

            var inactiveSubscription =
                await GetInactiveSubscriptionOfUserIdAsync(streamingService.Name, user.Id);
            if (inactiveSubscription != null)
            {
                var subscription = await _subscriptionRepository
                    .GetSubscriptionWithGivenIdAsync(inactiveSubscription.Id);

                if (subscription == null) throw new SubscriptionsDoesNotExistsException(inactiveSubscription.Id);
                var subscriptionConfirmation = new SubscriptionConfirmation()
                {
                    StartTime = today,
                    EndTime = today.AddMonths(dto.DurationInMonth),
                    PaymentMethod = dto.PaymentMethod,
                    Price = CalculateAmountOfConfirmation(streamingService.DefaultPrice, user),
                    Subscription = subscription,
                    User = user,
                    UserId = user.Id,
                    SubscriptionId = subscription.Id,
                };
                await _subscriptionConfirmationRepository.AddSubscriptionConfirmationAsync(subscriptionConfirmation);
            }
            else
            {
                var subscription = new Subscription
                {
                    StreamingServiceId = dto.StreamingServiceId,
                    StreamingService = streamingService
                };
                await _subscriptionRepository.AddSubscriptionAsync(subscription);

                var confirmation = new SubscriptionConfirmation
                {
                    StartTime = today,
                    EndTime = today.AddMonths(dto.DurationInMonth),
                    PaymentMethod = dto.PaymentMethod,
                    Price = CalculateAmountOfConfirmation(streamingService.DefaultPrice, user),
                    Subscription = subscription,
                    SubscriptionId = subscription.Id,
                    User = user,
                    UserId = user.Id
                };
                await _subscriptionConfirmationRepository.AddSubscriptionConfirmationAsync(confirmation);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task CancelSubscriptionWithGivenIdAsync(int subscriptionId)
    {
        if (subscriptionId <= 0) throw new ArgumentException("Subscription id can not be equal or smaller than 0.");

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existing = await _subscriptionRepository.GetSubscriptionWithGivenIdAsync(subscriptionId);
            if (existing == null)
                throw new SubscriptionsDoesNotExistsException(subscriptionId);


            await _subscriptionRepository.RemoveAsync(subscriptionId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<SubscriptionWithConfirmationsDTO>> GetAllSubscriptionsWithConfirmations()
    {
        var subscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();
        return subscriptions.Select(s => new SubscriptionWithConfirmationsDTO()
        {
            Subscription = new SubscriptionDTO()
            {
                Id = s.Id,
                DaysLeft = s.DurationInDays,
                StreamingServiceName = s.StreamingService.Name,
                AmountPaid = s.Confirmations.Sum(sc => sc.Price) // Total amount paid.
            },
            Confirmations = s.Confirmations.Select(c => new SubscriptionConfirmationDTO()
            {
                Id = c.Id,
                DurationInDays = CalculateRemainingDaysOfConfirmation(c.EndTime),
                PaymentMethod = c.PaymentMethod,
                Price = c.Price,
                StreamingServiceName = c.Subscription.StreamingService.Name,
                UserId = c.UserId,
                UserCountry = c.User.Country,
                UserStatus = c.User.Status.ToString()
            }).ToList()
        }).ToList();
    }

    public async Task<IEnumerable<SubscriptionDTO>> GetActiveSubscriptionsOfUserIdAsync(int userId)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");

        if (await _userRepository.GetUserWithGivenId(userId) == null)
            throw new UserDoesNotExistsException(userId);

        var confirmations = await _subscriptionRepository.GetUserSubscriptionConfirmationsAsync(userId);
        var today = DateOnly.FromDateTime(DateTime.Now);

        var activeConfirmations = confirmations
            .GroupBy(c => c.SubscriptionId)
            .Select(g => g.OrderByDescending(c => c.StartTime).First())
            .Where(latest => latest.EndTime > today);

        var result = activeConfirmations.Select(c => new SubscriptionDTO
        {
            Id = c.SubscriptionId,
            DaysLeft = CalculateRemainingDaysOfConfirmation(c.EndTime),
            StreamingServiceName = c.Subscription.StreamingService.Name,
            AmountPaid = c.Price
        });

        return result;
    }

    public async Task<IEnumerable<SubscriptionDTO>> GetInactiveSubscriptionsOfUserIdAsync(int userId)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");

        if (await _userRepository.GetUserWithGivenId(userId) == null)
            throw new UserDoesNotExistsException(userId);

        var confirmations = await _subscriptionRepository.GetUserSubscriptionConfirmationsAsync(userId);
        var today = DateOnly.FromDateTime(DateTime.Now);

        var inactiveConfirmations = confirmations
            .GroupBy(c => c.SubscriptionId)
            .Select(g => g.OrderByDescending(c => c.StartTime).First())
            .Where(latest => latest.EndTime < today);

        var result = inactiveConfirmations.Select(c => new SubscriptionDTO
        {
            Id = c.SubscriptionId,
            DaysLeft = CalculateRemainingDaysOfConfirmation(c.EndTime),
            StreamingServiceName = c.Subscription.StreamingService.Name,
            AmountPaid = c.Price
        });

        return result;
    }

    public async Task<SubscriptionDTO?> GetInactiveSubscriptionOfUserIdAsync(string streamingServiceName,
        int userId)
    {
        if (string.IsNullOrEmpty(streamingServiceName))
            throw new ArgumentException("Streaming service name can not be neither null or empty.");
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");

        if (await _userRepository.GetUserWithGivenId(userId) == null)
            throw new UserDoesNotExistsException(userId);

        var confirmations = await _subscriptionRepository.GetUserSubscriptionConfirmationsAsync(userId);
        var today = DateOnly.FromDateTime(DateTime.Now);

        var inactiveSubscription = confirmations
            .GroupBy(c => c.SubscriptionId)
            .Select(g => g.OrderByDescending(c => c.StartTime).First())
            .Where(latest => latest.EndTime < today)
            .FirstOrDefault(c => c.Subscription.StreamingService.Name == streamingServiceName);

        if (inactiveSubscription != null)
        {
            return new SubscriptionDTO
            {
                Id = inactiveSubscription.SubscriptionId,
                DaysLeft = CalculateRemainingDaysOfConfirmation(inactiveSubscription.EndTime),
                StreamingServiceName = inactiveSubscription.Subscription.StreamingService.Name,
                AmountPaid = inactiveSubscription.Price
            };
        }


        return null;
    }

    private static int CalculateRemainingDaysOfConfirmation(DateOnly endDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return endDate < today
            ? 0
            : endDate.DayNumber
              - today.DayNumber
              + 1;
    }

    public static decimal CalculateAmountOfConfirmation(decimal defaultPrice, User user)
    {
        decimal discount = 0;

        switch (user.Status)
        {
            case Status.Student:
                discount += 0.20m;
                break;
            case Status.Elder:
                discount += 0.10m;
                break;
        }

        switch (user.Country.Trim().ToUpperInvariant())
        {
            case "POLAND":
                discount += 0.05m;
                break;
            case "GERMANY":
                discount += 0.03m;
                break;
            case "FRANCE":
                discount += 0.02m;
                break;
        }

        return Math.Max(defaultPrice * (1 - discount), 0);
    }
}