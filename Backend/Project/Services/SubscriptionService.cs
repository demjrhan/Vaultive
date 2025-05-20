using Project.Context;
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
    

}