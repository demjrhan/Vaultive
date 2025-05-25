using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs.MediaContentDTOs;
using Project.DTOs.StreamingServiceDTOs;
using Project.DTOs.SubscriptionDTOs;
using Project.Exceptions;
using Project.Models;
using Project.Repositories;

namespace Project.Services;

public class StreamingServiceService
{
    private readonly ReviewRepository _reviewRepository;
    private readonly UserRepository _userRepository;
    private readonly StreamingServiceRepository _streamingServiceRepository;
    private readonly SubscriptionConfirmationRepository _subscriptionConfirmationRepository;
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly MediaContentRepository _mediaContentRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;
    private readonly MasterContext _context;

    public StreamingServiceService(
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

    /* Returns all the streaming services with their supported movies and subscriptions. */
    public async Task<List<StreamingServiceDetailedDTO>> GetAllStreamingServicesDetailedAsync()
    {
        var streamingServices = await _streamingServiceRepository.GetAllStreamingServicesAsync();
        return streamingServices.Select(ss => new StreamingServiceDetailedDTO()
        {
            Service = new StreamingServiceDTO()
            {
                Id = ss.Id,
                Name = ss.Name,
                WebsiteLink = ss.WebsiteLink,
                LogoImage = ss.LogoImage,
                Country = ss.Country,
                DefaultPrice = ss.DefaultPrice,
                Description = ss.Description,
            },
            MediaContents = ss.MediaContents.Select(m => new MediaContentDTO()
            {
                Id = m.Id,
                Title = m.Title,
                Country = m.Country,
                Description = m.Description,
                Duration = m.Duration,
                OriginalLanguage = m.OriginalLanguage,
                ReleaseDate = m.ReleaseDate
            }).ToList(),
            Subscriptions = ss.Subscriptions.Select(s => new SubscriptionDTO()
            {
                Id = s.Id,
                AmountPaid = s.Confirmations.MaxBy(sc => sc.StartTime)!.Price,
                DaysLeft = s.DurationInDays,
                StreamingServiceName = s.StreamingService.Name
            }).ToList()
        }).ToList();
    }

    /* Returns all the streaming services. */
    public async Task<List<StreamingServiceDTO>> GetAllStreamingServicesAsync()
    {
        var streamingServices = await _streamingServiceRepository.GetAllStreamingServicesAsync();
        return streamingServices.Select(ss => new StreamingServiceDTO()
        {
            Id = ss.Id,
            Name = ss.Name,
            WebsiteLink = ss.WebsiteLink,
            LogoImage = ss.LogoImage,
            Country = ss.Country,
            DefaultPrice = ss.DefaultPrice,
            Description = ss.Description
        }).ToList();
    }

    /* Remove the streaming service with the given id */
    public async Task RemoveStreamingServiceWithGivenIdAsync(int streamingServiceId)
    {
        if (streamingServiceId <= 0)
            throw new ArgumentException("Streaming service id can not be equal or smaller than 0.");
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _streamingServiceRepository.RemoveAsync(streamingServiceId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /* Adding new streaming service data to database. */
    public async Task AddStreamingServiceAsync(CreateStreamingServiceDTO streamingServiceDto)
    {
        if (streamingServiceDto == null)
            throw new ArgumentNullException(nameof(streamingServiceDto));

        /* Streaming Service name must be unique. */
        if (await _context.StreamingServices.AnyAsync(m => m.Name == streamingServiceDto.Name))
            throw new StreamingServiceNameMustBeUniqueException(streamingServiceDto.Name);

        List<MediaContent> mediaContents = new List<MediaContent>();
        var requestedIds = streamingServiceDto.SupportedMediaContents;

        if (requestedIds.Any())
        {
            if (requestedIds.Any(id => id <= 0))
                throw new ArgumentException(
                    "All supported media content IDs must be positive integers.",
                    nameof(streamingServiceDto.SupportedMediaContents));

            mediaContents = await _context.MediaContents
                .Where(m => requestedIds.Contains(m.Id))
                .ToListAsync();

            var foundIds = mediaContents.Select(m => m.Id);
            var missingIds = requestedIds.Except(foundIds).ToList();

            if (missingIds.Any())
                throw new MediaContentDoesNotExistsException(missingIds);
        }


        ValidateStreamingService(
            name: streamingServiceDto.Name,
            countryCode: streamingServiceDto.Country,
            description: streamingServiceDto.Description,
            defaultPrice: streamingServiceDto.DefaultPrice,
            websiteLink: streamingServiceDto.WebsiteLink
        );


        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _streamingServiceRepository.AddAsync(new StreamingService()
            {
                Country = streamingServiceDto.Country,
                DefaultPrice = streamingServiceDto.DefaultPrice,
                Description = streamingServiceDto.Description,
                WebsiteLink = streamingServiceDto.WebsiteLink,
                LogoImage = streamingServiceDto.LogoImage,
                Name = streamingServiceDto.Name,
                MediaContents = mediaContents
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

    private void ValidateStreamingService(
        string name,
        string countryCode,
        string description,
        decimal defaultPrice,
        string websiteLink)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2 || name.Length > 100)
            throw new ArgumentException("Name must be 2–100 characters.", nameof(name));

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Country is required.", nameof(countryCode));

        if (countryCode.Length != 2)
            throw new ArgumentException("Country must be an ISO alpha-2 code (e.g. \"US\", \"PL\").",
                nameof(countryCode));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description must not be empty.", nameof(description));

        if (defaultPrice < 0m || defaultPrice > 500m)
            throw new ArgumentException("Default price must be between 0 and 500.", nameof(defaultPrice));

        // Website: must be http(s) URL
        if (!Uri.TryCreate(websiteLink, UriKind.Absolute, out var uri))
            throw new ArgumentException("Website link must be a valid URL.", nameof(websiteLink));

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            throw new ArgumentException("Website link must use http or https.", nameof(websiteLink));
    }
    
    
}