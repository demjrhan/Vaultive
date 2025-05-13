using backend.Exceptions;
using Project.DTOs;
using Project.Modules.Enumerations;
using Project.Repositories;

namespace Project.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly SubscriptionConfirmationRepository _subscriptionConfirmationRepository;

    public UserService(UserRepository userRepository,
        SubscriptionConfirmationRepository subscriptionConfirmationRepository)
    {
        _userRepository = userRepository;
        _subscriptionConfirmationRepository = subscriptionConfirmationRepository;
    }

    public async Task<UserWithSubscriptionsDTO> GetUserWithSubscriptions(int userId)
    {
        var user = await _userRepository.GetUserWithIdAsync(userId);
        var subscriptionConfirmations =
            await _subscriptionConfirmationRepository.GetUsersSubscriptionConfirmationsAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        if (subscriptionConfirmations == null)
        {
            throw new SubscriptionsNotFoundException(userId);
        }

        return new UserWithSubscriptionsDTO()
            {
                User = new UserResponseDTO()
                {
                    Country = user.Country,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Nickname = user.Nickname,
                    Status = user.Status.ToString()
                },
                Subscriptions = subscriptionConfirmations.Select(sc => new SubscriptionResponseDTO
                {
                    DaysLeft = (sc.EndTime - DateTime.Today).Days,
                    Price = sc.Amount,
                    StreamingServiceName = sc.Subscription.StreamingService.Name
                }).ToList()
            };
    }
}