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
    private readonly MediaContentRepository _mediaContentRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;
    private readonly MasterContext _context;

    public UserService(
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

    public async Task RemoveUserWithGivenIdAsync(int userId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _userRepository.RemoveAsync(userId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new RemoveDataFailedException(ex);
        }
    }

    /* Adding new movie data to database. */
    public async Task AddUserAsync(CreateUserDTO userDto)
    {
        if (userDto == null)
            throw new ArgumentNullException(nameof(userDto));

        if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            throw new EmailAlreadyExistsException(userDto.Email);

        if (await _context.Users.AnyAsync(u => u.Nickname == userDto.Nickname))
            throw new NicknameAlreadyExistsException(userDto.Nickname);

        /* Before starting the process we are validating if the given genres are parse-able to actual Genre enumeration class. */
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
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new AddDataFailedException(ex);
        }
    }

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