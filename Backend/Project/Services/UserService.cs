using Project.DTOs;
using Project.DTOs.UserDTOs;
using Project.DTOs.WatchHistoryDTOs;
using Project.Exceptions;
using Project.Models;
using Project.Models.Enumerations;
using Project.Repositories;

namespace Project.Services;

public class UserService
{
    private readonly MovieRepository _movieRepository;
    private readonly UserRepository _userRepository;
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly SubscriptionService _subscriptionService;
    private readonly WatchHistoryRepository _watchHistoryRepository;

    public UserService(MovieRepository movieRepository,
        UserRepository userRepository,
        SubscriptionRepository subscriptionRepository,
        SubscriptionService subscriptionService,
        WatchHistoryRepository watchHistoryRepository)
    {
        _movieRepository = movieRepository;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
        _subscriptionService = subscriptionService;
        _watchHistoryRepository = watchHistoryRepository;
    }

    public async Task<UserWithSubscriptionsDTO> GetUserWithSubscriptions(int userId)
    {
        var user = await _userRepository.GetUserWithIdAsync(userId);
        if (user == null)
            throw new UserNotFoundException(userId);
        var subscriptions = await _subscriptionService.GetActiveSubscriptionsOfUserIdAsync(userId);


       

        return new UserWithSubscriptionsDTO
        {
            User = new UserResponseDTO
            {
                Country = user.Country,
                Nickname = user.Nickname,
                Status = user.Status.ToString()
            },
            Subscriptions = subscriptions.ToList()
        };
    }


    public async Task<UserWithWatchHistoryDTO> GetUserWithWatchHistory(int userId)
    {
        var user = await _userRepository.GetUserWithIdAsync(userId);
        var watchHistory = await _watchHistoryRepository.GetWatchHistoriesOfUserId(userId);

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
    public async Task AddUserAsync(CreateUserDTO user)
    {
        if (string.IsNullOrWhiteSpace(user.Nickname))
            throw new ArgumentException("Nickname is required.");

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(user.Country))
            throw new ArgumentException("Country is required.");

        var existingByEmail = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existingByEmail != null)
            throw new EmailAlreadyExistsException(user.Email);

        var allUsers = await _userRepository.GetAllUsersAsync();
        if (allUsers.Any(u => u.Nickname == user.Nickname))
            throw new NicknameAlreadyExistsException(user.Nickname);

        if (!Enum.IsDefined(typeof(Status), user.Status))
            throw new InvalidUserStatusException(user.Status.ToString());

        var createdUser = new User()
        {
            Country = user.Country,
            Email = user.Email,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Nickname = user.Nickname,
            Status = user.Status
        };

        await _userRepository.AddUserAsync(createdUser);
        await _userRepository.SaveChangesAsync();

    }
    public async Task DeleteUserAsync(int userId)
    {
        var existing = await _userRepository.GetUserWithIdAsync(userId);
        if (existing == null)
        {
            throw new UserNotFoundException(userId);
        }

        await _userRepository.DeleteUserAsync(userId);
        await _userRepository.SaveChangesAsync();

    }

    public async Task UpdateUserAsync(UpdateUserDTO updatedUser)
    {
        var existing = await _userRepository.GetUserWithIdAsync(updatedUser.Id);
        if (existing == null)
        {
            throw new UserNotFoundException(updatedUser.Id);
        }

        if (string.IsNullOrWhiteSpace(updatedUser.Nickname))
            throw new ArgumentException("Nickname is required.");

        if (string.IsNullOrWhiteSpace(updatedUser.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(updatedUser.Country))
            throw new ArgumentException("Country is required.");

        var userWithEmail = await _userRepository.GetUserByEmailAsync(updatedUser.Email);
        if (userWithEmail != null && userWithEmail.Id != updatedUser.Id)
            throw new EmailAlreadyExistsException(updatedUser.Email);

        var allUsers = await _userRepository.GetAllUsersAsync();
        if (allUsers.Any(u => u.Nickname == updatedUser.Nickname && u.Id != updatedUser.Id))
            throw new NicknameAlreadyExistsException(updatedUser.Nickname);

        if (!Enum.IsDefined(typeof(Status), updatedUser.Status))
            throw new InvalidUserStatusException(updatedUser.Status.ToString());

        if (!IsUserUpdateRequired(existing, updatedUser))
            throw new NoChangesDetectedException();

        await _userRepository.UpdateUserAsync(existing, updatedUser);
        await _userRepository.SaveChangesAsync();
    }
    private bool IsUserUpdateRequired(User existing, UpdateUserDTO updated)
    {
        bool AreEqual(string? a, string? b) =>
            string.Equals(a?.Trim(), b?.Trim(), StringComparison.OrdinalIgnoreCase);
        return
            !AreEqual(existing.Firstname, updated.Firstname) ||
            !AreEqual(existing.Lastname, updated.Lastname) ||
            !AreEqual(existing.Nickname, updated.Nickname) ||
            !AreEqual(existing.Email, updated.Email) ||
            !AreEqual(existing.Country, updated.Country) ||
            existing.Status != updated.Status;
    }

}