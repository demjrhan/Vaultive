using Project.Context;
using Project.DTOs.SubscriptionDTOs;
using Project.Exceptions;
using Project.Models;
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
            throw new UserNotFoundException(userId);

        var confirmations = await _subscriptionRepository.GetUserSubscriptionConfirmationsAsync(userId);
        var today = DateOnly.FromDateTime(DateTime.Now);

        var activeConfirmations = confirmations
            .GroupBy(c => c.SubscriptionId)
            .Select(g => g.OrderByDescending(c => c.StartTime).First())
            .Where(latest => latest.EndTime > today);

        var result = activeConfirmations.Select(c => new SubscriptionDTO
        {
            Id = c.Id,
            DaysLeft = c.Subscription.DurationInDays,
            StreamingServiceName = c.Subscription.StreamingService.Name,
        });

        return result;
    }

    public int CalculateRemainingDaysOfConfirmation(DateOnly endDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return endDate < today
            ? 0
            : endDate.DayNumber
              - today.DayNumber
              + 1;
    }
}