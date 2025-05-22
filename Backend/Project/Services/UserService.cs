using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs.ReviewDTOs;
using Project.DTOs.SubscriptionDTOs;
using Project.DTOs.UserDTOs;
using Project.DTOs.WatchHistoryDTOs;
using Project.Exceptions;
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
    public async Task UpdateUserWithGivenIdAsync(int userId, UpdateUserDTO userDto)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");

        if (userDto == null)
            throw new ArgumentNullException(nameof(userDto));

        if (await _context.Users.AnyAsync(u => u.Email == userDto.Email && u.Id != userId))
            throw new EmailAlreadyExistsException(userDto.Email);

        if (await _context.Users.AnyAsync(u => u.Nickname == userDto.Nickname && u.Id != userId))
            throw new NicknameAlreadyExistsException(userDto.Nickname);


        var user = await _userRepository.GetUserWithGivenId(userId);
        if (user == null) throw new UserNotFoundException(userId);

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

    public async Task<List<UserDetailedResponseDTO>> GetAllUsersDetailedAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();

        return users.Select(u => new UserDetailedResponseDTO()
        {
            Country = u.Country,
            Firstname = u.Firstname,
            Lastname = u.Lastname,
            Nickname = u.Nickname,
            Status = u.Status.ToString(),
            Reviews = u.Reviews.Select(r => new ReviewResponseDTO()
            {
                Id = r.Id,
                Comment = r.Comment,
                MediaTitle = r.MediaContent.Title,
                Nickname = r.User.Nickname,
                WatchedOn = r.WatchHistory.WatchDate
            }).ToList(),
            Confirmations = u.Confirmations.Select(c => new SubscriptionConfirmationResponseDTO()
            {
                Id = c.Id,
                DurationInDays = c.Subscription.DurationInDays,
                PaymentMethod = c.PaymentMethod,
                Price = c.Price,
                StreamingServiceName = c.Subscription.StreamingService.Name,
                SubscriptionId = c.SubscriptionId,
                UserCountry = c.User.Country,
                UserId = c.UserId,
                UserStatus = c.User.Status.ToString()
            }).ToList(),
            WatchHistories = u.WatchHistories.Select(wh => new WatchHistoryResponseDTO()
            {
                MediaId = wh.MediaId,
                MediaTitle = wh.MediaContent.Title,
                TimeLeftOf = wh.TimeLeftOf,
                WatchDate = wh.WatchDate
            }).ToList()
        }).ToList();
    }

    /* Make user watch a media content */
    public async Task WatchMediaContentAsync(int userId, int mediaId)
    {
        if (userId <= 0) throw new ArgumentException("User id can not be equal or smaller than 0.");
        if (mediaId <= 0) throw new ArgumentException("Media id can not be equal or smaller than 0.");
        
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var mediaContent = await _mediaContentRepository.GetMediaContentWithGivenIdAsync(mediaId);
            if (mediaContent == null) throw new MediaContentDoesNotExistsException(mediaId);

            var user = await _userRepository.GetUserWithGivenId(userId);
            if (user == null) throw new UserNotFoundException(userId);

            if (user.WatchHistories.Any(wh => wh.MediaId == mediaId && wh.TimeLeftOf == 0))
                throw new MediaContentAlreadyWatchedException(user.Nickname, mediaContent.Title);

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