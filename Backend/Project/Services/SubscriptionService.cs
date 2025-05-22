using Project.Context;
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
    

}