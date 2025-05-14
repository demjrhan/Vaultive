using backend.Exceptions;
using Project.DTOs;
using Project.Modules.Enumerations;
using Project.Repositories;

namespace Project.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly SubscriptionRepository _subscriptionRepository;

    public UserService(UserRepository userRepository,
        SubscriptionRepository subscriptionRepository)
    {
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<UserWithSubscriptionsDTO> GetUserWithSubscriptions(int userId)
    {
        var user = await _userRepository.GetUserWithIdAsync(userId);
        var activeSubscriptions = await _subscriptionRepository.GetUserActiveSubscriptionsAsync(userId);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        if (activeSubscriptions == null)
        {
            throw new SubscriptionsNotFoundException(userId);
        }

        var subscriptionDtos = await Task.WhenAll(activeSubscriptions.Select(async sc =>
        {
            var confirmation = await _subscriptionRepository.GetConfirmationDetailsForSubscription(sc);

            return confirmation == null ? null : new SubscriptionResponseDTO
            {
                DaysLeft = sc.DurationInDays,
                Price = confirmation.Price, 
                StreamingServiceName = sc.StreamingService.Name
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
            Subscriptions = subscriptionDtos.Where(s => s != null)!
        };
    }

}