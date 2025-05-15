using Project.DTOs;
using Project.Exceptions;
using Project.Repositories;

namespace Project.Services;

public class UserService
{
    private readonly MovieRepository _movieRepository;
    private readonly UserRepository _userRepository;
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;

    public UserService(MovieRepository movieRepository,
        UserRepository userRepository,
        SubscriptionRepository subscriptionRepository,
        WatchHistoryRepository watchHistoryRepository)
    {
        _movieRepository = movieRepository;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
        _watchHistoryRepository = watchHistoryRepository;
    }

    public async Task<UserWithSubscriptionsDTO> GetUserWithSubscriptions(int userId)
    {
        var user = await _userRepository.GetUserWithIdAsync(userId);
        var activeSubscriptions = await _subscriptionRepository.GetActiveSubscriptionsForUserIdAsync(userId);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        if (activeSubscriptions == null)
        {
            throw new SubscriptionsNotFoundException(userId);
        }

        var subscriptionDtos = await Task.WhenAll(activeSubscriptions.Select(async s =>
        {
            var confirmation = await _subscriptionRepository.GetConfirmationDetailsForSubscription(s);
            if (confirmation == null)
            {
                throw new SubscriptionConfirmationNotFoundException(s.Id);
            }

            return new SubscriptionResponseDTO
            {
                DaysLeft = s.DurationInDays,
                Price = confirmation.Price,
                StreamingServiceName = s.StreamingService.Name
            };
        }));

        return new UserWithSubscriptionsDTO
        {
            User = new UserResponseDTO
            {
                Country = user.Country,
                Nickname = user.Nickname,
                Status = user.Status.ToString()
            },
            Subscriptions = subscriptionDtos.ToList()
        };
    }

    public async Task<UserWithWatchHistoryDTO> GetUserWithWatchHistory(int userId)
    {
        var user = await _userRepository.GetUserWithIdAsync(userId);
        var watchHistory = await _watchHistoryRepository.GetWatchHistoryForUsedId(userId);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        if (watchHistory == null)
        {
            throw new WatchHistoryNotFoundException(userId);
        }

        return new UserWithWatchHistoryDTO()
        {
            User = new UserResponseDTO()
            {
                Country = user.Country,
                Nickname = user.Nickname,
                Status = user.Status.ToString()
            },
            WatchHistory = watchHistory.Select(wh => new WatchHistoryResponseDTO()
            {
                MediaTitle = wh.MediaTitle,
                TimeLeftOf = wh.TimeLeftOf,
                WatchDate = wh.WatchDate
            }).ToList()
        };
    }
}