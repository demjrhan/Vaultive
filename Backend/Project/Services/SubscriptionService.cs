using Project.Context;
using Project.DTOs.SubscriptionDTOs;
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
    public async Task<IEnumerable<SubscriptionResponseDTO>> GetActiveSubscriptionsOfUserIdAsync(int userId)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");

        var confirmations = await _subscriptionRepository.GetUserSubscriptionConfirmationsAsync(userId);
        var today = DateOnly.FromDateTime(DateTime.Now);
        
        var activeConfirmations = confirmations
            .GroupBy(c => c.SubscriptionId)
            .Select(g => g.OrderByDescending(c => c.StartTime).First())
            .Where(latest => latest.EndTime > today);

        var result = activeConfirmations.Select(c => new SubscriptionResponseDTO
        {
            Id = c.Id,
            DaysLeft = c.Subscription.DurationInDays,
            StreamingServiceName = c.Subscription.StreamingService.Name,
            Price = c.Price
        });

        return result;
    }

}