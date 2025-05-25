using Project.Context;
using Project.DTOs.MediaContentDTOs;
using Project.DTOs.StreamingServiceDTOs;
using Project.DTOs.SubscriptionDTOs;
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
        if (streamingServiceId <= 0) throw new ArgumentException("Streaming service id can not be equal or smaller than 0.");
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
}