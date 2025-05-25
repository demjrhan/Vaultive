using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs.ReviewDTOs;
using Project.DTOs.SubscriptionDTOs;
using Project.DTOs.UserDTOs;
using Project.DTOs.WatchHistoryDTOs;
using Project.Exceptions;
using Project.Helper;
using Project.Models;
using Project.Models.Enumerations;
using Project.Repositories;

namespace Project.Services;

public class UserService
{
    private readonly ReviewRepository _reviewRepository;
    private readonly UserRepository _userRepository;
    private readonly StreamingServiceRepository _streamingServiceRepository;
    private readonly SubscriptionConfirmationRepository _subscriptionConfirmationRepository;
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly SubscriptionService _subscriptionService;
    private readonly MediaContentRepository _mediaContentRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;
    private readonly MasterContext _context;
    private static readonly Random _random = new Random();

    public UserService(
        MasterContext context,
        ReviewRepository reviewRepository,
        UserRepository userRepository,
        MediaContentRepository mediaContentRepository,
        WatchHistoryRepository watchHistoryRepository,
        SubscriptionRepository subscriptionRepository,
        SubscriptionService subscriptionService,
        StreamingServiceRepository streamingServiceRepository,
        SubscriptionConfirmationRepository subscriptionConfirmationRepository)
    {
        _context = context;
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _mediaContentRepository = mediaContentRepository;
        _watchHistoryRepository = watchHistoryRepository;
        _subscriptionRepository = subscriptionRepository;
        _subscriptionService = subscriptionService;
        _streamingServiceRepository = streamingServiceRepository;
        _subscriptionConfirmationRepository = subscriptionConfirmationRepository;
    }

    /* Remove user with given id */
    public async Task RemoveUserWithGivenIdAsync(int userId)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _userRepository.RemoveAsync(userId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /* Adding new user data to database. */
    public async Task AddUserAsync(CreateUserDTO userDto)
    {
        if (userDto == null)
            throw new ArgumentNullException(nameof(userDto));

        if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            throw new EmailAlreadyExistsException(userDto.Email);

        if (await _context.Users.AnyAsync(u => u.Nickname == userDto.Nickname))
            throw new NicknameAlreadyExistsException(userDto.Nickname);

        ValidateUser(
            firstname: userDto.Firstname,
            lastname: userDto.Lastname,
            nickname: userDto.Nickname,
            email: userDto.Email,
            country: userDto.Country,
            status: userDto.Status);

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var status = ParseStatus(userDto.Status);

            await _userRepository.AddAsync(new User()
            {
                Firstname = userDto.Firstname,
                Lastname = userDto.Lastname,
                Nickname = userDto.Nickname,
                Email = userDto.Email,
                Country = userDto.Country,
                Status = status
            });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /* Update the user with the given id */
    public async Task UpdateUserWithGivenIdAsync(UpdateUserDTO userDto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (userDto.Id <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");

            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email && u.Id != userDto.Id))
                throw new EmailAlreadyExistsException(userDto.Email);

            if (await _context.Users.AnyAsync(u => u.Nickname == userDto.Nickname && u.Id != userDto.Id))
                throw new NicknameAlreadyExistsException(userDto.Nickname);


            var user = await _userRepository.GetUserWithGivenId(userDto.Id) ??
                       throw new UserNotFoundException(userDto.Id);

            ValidateUser(
                firstname: userDto.Firstname,
                lastname: userDto.Lastname,
                nickname: userDto.Nickname,
                email: userDto.Email,
                country: userDto.Country,
                status: userDto.Status);

            var status = ParseStatus(userDto.Status);

            user.Firstname = userDto.Firstname;
            user.Lastname = userDto.Lastname;
            user.Nickname = userDto.Nickname;
            user.Email = userDto.Email;
            user.Country = userDto.Country;
            user.Status = status;


            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /* Get all users with detail, like watch history etc. */
    public async Task<List<UserDetailedDTO>> GetAllUsersDetailedAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var result = new List<UserDetailedDTO>();
        foreach (var user in users)
        {
            var activeSubscriptions = await _subscriptionService.GetActiveSubscriptionsOfUserIdAsync(user.Id);
            var userDetailed = new UserDetailedDTO()
            {
                Id = user.Id,
                Country = user.Country,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Nickname = user.Nickname,
                Status = user.Status.ToString(),
                Reviews = user.Reviews.Select(r => new ReviewDTO()
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    MediaTitle = r.MediaContent.Title,
                    Nickname = r.User.Nickname,
                    WatchedOn = r.WatchHistory.WatchDate
                }).ToList(),
                ActiveSubscriptions = await _subscriptionService.GetActiveSubscriptionsOfUserIdAsync(user.Id),
                WatchHistories = user.WatchHistories.Select(wh => new WatchHistoryDTO()
                {
                    MediaId = wh.MediaId,
                    MediaTitle = wh.MediaContent.Title,
                    TimeLeftOf = wh.TimeLeftOf,
                    WatchDate = wh.WatchDate
                }).ToList()
            };

            result.Add(userDetailed);
        }

        return result;
    }

    /* Get user with detail, like watch history etc. */
    public async Task<UserDetailedDTO> GetUserWithGivenIdAsync(int userId)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");
        var user = await _userRepository.GetUserWithGivenId(userId) ??
                   throw new UserNotFoundException(userId);

        return new UserDetailedDTO()
        {
            Id = user.Id,
            Country = user.Country,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Nickname = user.Nickname,
            Status = user.Status.ToString(),
            Reviews = user.Reviews.Select(r => new ReviewDTO()
            {
                Id = r.Id,
                Comment = r.Comment,
                MediaTitle = r.MediaContent.Title,
                Nickname = r.User.Nickname,
                WatchedOn = r.WatchHistory.WatchDate
            }).ToList(),
            ActiveSubscriptions = await _subscriptionService.GetActiveSubscriptionsOfUserIdAsync(user.Id),
            WatchHistories = user.WatchHistories.Select(wh => new WatchHistoryDTO()
            {
                MediaId = wh.MediaId,
                MediaTitle = wh.MediaContent.Title,
                TimeLeftOf = wh.TimeLeftOf,
                WatchDate = wh.WatchDate
            }).ToList()
        };
    }

    /* Make user watch a media content */
    public async Task WatchMediaContentAsync(int userId, int mediaId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");
            if (mediaId <= 0) throw new ArgumentException("Media id can not be equal or smaller than 0.");

            var mediaContent = await _mediaContentRepository.GetMediaContentWithGivenIdAsync(mediaId) ??
                               throw new MediaContentDoesNotExistsException(mediaId);

            var user = await _userRepository.GetUserWithGivenId(userId) ?? throw new UserNotFoundException(userId);


            if (user.WatchHistories.Any(wh => wh.MediaId == mediaId && wh.TimeLeftOf == 0))
                throw new MediaContentAlreadyWatchedException(user.Nickname, mediaContent.Title);

            var canWatch = await CanUserWatchContent(user, mediaContent);
            if (!canWatch) throw new UserCanNotWatchMediaContentException(userId);

            var activeSubscriptions = await _subscriptionService.GetActiveSubscriptionsOfUserIdAsync(userId);
            if (!activeSubscriptions.Any())
                throw new UserHasNoActiveSubscriptionException(user.Nickname,
                    $"Can not watch media content unless have subscription for the streaming services it supports.");

            var existing = await _context.WatchHistories
                .SingleOrDefaultAsync(wh => wh.UserId == userId && wh.MediaId == mediaId);

            if (existing != null)
            {
                if (existing.TimeLeftOf == 0)
                    throw new MediaContentAlreadyWatchedException(user.Nickname, mediaContent.Title);

                existing.TimeLeftOf = _random.Next(0, existing.TimeLeftOf);
                existing.WatchDate = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                user.WatchHistories.Add(new WatchHistory
                {
                    MediaContent = mediaContent,
                    TimeLeftOf = _random.Next(0, mediaContent.Duration),
                    WatchDate = DateOnly.FromDateTime(DateTime.Now)
                });
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

    /* Make user add review to media content */
    public async Task AddReviewToMediaContentAsync(AddReviewDTO addReviewDto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (addReviewDto.UserId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");
            if (addReviewDto.MediaId <= 0) throw new ArgumentException("Media id can not be equal or smaller than 0.");

            var mediaContent = await _mediaContentRepository.GetMediaContentWithGivenIdAsync(addReviewDto.MediaId) ??
                               throw new MediaContentDoesNotExistsException(addReviewDto.MediaId);

            var user = await _userRepository.GetUserWithGivenId(addReviewDto.UserId) ??
                       throw new UserNotFoundException(addReviewDto.UserId);


            var watchHistory =
                await _watchHistoryRepository.GetWatchHistoryOfUserToGivenMediaIdAsync(user.Id, mediaContent.Id) ??
                throw new WatchHistoryNotFoundException(user.Id);

            var existingReview =
                await _reviewRepository.GetReviewOfUserToMediaContentAsync(user.Id, mediaContent.Id);

            if (existingReview != null)
                throw new DuplicateReviewException(user.Nickname, mediaContent.Title);

            var review = new Review
            {
                Comment = addReviewDto.Comment,
                UserId = user.Id,
                User = user,
                MediaContent = mediaContent,
                WatchHistory = watchHistory
            };

            await _reviewRepository.AddReviewAsync(review);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /* Make user subscribe to streaming service*/
    public async Task SubscribeAsync(AddSubscriptionDTO dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var streamingService = await _streamingServiceRepository.GetStreamingServiceByIdAsync(dto.StreamingServiceId);
            if (streamingService == null)
                throw new StreamingServiceNotFoundException(new[] { dto.StreamingServiceId });

            var user = await _userRepository.GetUserWithGivenId(dto.UserId);
            if (user == null)
                throw new UserNotFoundException(dto.UserId);

            var activeSubscriptions = await _subscriptionService.GetActiveSubscriptionsOfUserIdAsync(user.Id);
            if (activeSubscriptions.Any(ss => ss.StreamingServiceName == streamingService.Name))
                throw new SubscriptionAlreadyExistsException(user.Id, streamingService.Id);

            var subscription = new Subscription
            {
                StreamingServiceId = dto.StreamingServiceId,
                StreamingService = streamingService
            };
            await _subscriptionRepository.AddSubscriptionAsync(subscription);

            var today = DateOnly.FromDateTime(DateTime.Now);
            var confirmation = new SubscriptionConfirmation
            {
                StartTime = today,
                EndTime = today.AddMonths(dto.DurationInMonth),
                PaymentMethod = dto.PaymentMethod,
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
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    /* Validations */
    private async Task<bool> CanUserWatchContent(User user, MediaContent mediaContent)
    {
        var activeSubscriptions = await _subscriptionService
            .GetActiveSubscriptionsOfUserIdAsync(user.Id);

        var activeNames = activeSubscriptions
            .Select(asc => asc.StreamingServiceName);

        var contentNames = mediaContent
            .StreamingServices
            .Select(ss => ss.Name);

        return activeNames
            .Intersect(contentNames)
            .Any();
    }
    
    private static Status ParseStatus(string status)
    {
        return Enum.TryParse<Status>(status, true, out var result)
            ? result
            : throw new InvalidUserStatusException(status);
    }

    private void ValidateUser(
        string? firstname,
        string? lastname,
        string nickname,
        string email,
        string country,
        string status)
    {
        // First name: optional, but if provided must be 2–50 chars
        if (!string.IsNullOrWhiteSpace(firstname) &&
            (firstname.Length < 2 || firstname.Length > 50))
        {
            throw new ArgumentException("First name must be 2–50 characters when provided.", nameof(firstname));
        }

        // Last name: optional, but if provided must be 2–50 chars
        if (!string.IsNullOrWhiteSpace(lastname) &&
            (lastname.Length < 2 || lastname.Length > 50))
        {
            throw new ArgumentException("Last name must be 2–50 characters when provided.", nameof(lastname));
        }

        // Nickname: required, 2–30 chars
        if (string.IsNullOrWhiteSpace(nickname) ||
            nickname.Length < 2 || nickname.Length > 30)
        {
            throw new ArgumentException("Nickname is required and must be 2–30 characters.", nameof(nickname));
        }

        // Email: required, must be valid format
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        if (!IsValidEmail(email))
        {
            throw new ArgumentException("Email is not in a valid format.", nameof(email));
        }

        // Country: required, non-empty
        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country is required.", nameof(country));
        }

        if (country.Length > 2)
        {
            throw new ArgumentException("Country name must fit into alpha-2 code. e.g 'TR' 'PL", nameof(country));
        }

        // Status: required, non-empty
        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException("Status is required.", nameof(status));
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            // System.Net.Mail.MailAddress will throw if invalid
            var _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}