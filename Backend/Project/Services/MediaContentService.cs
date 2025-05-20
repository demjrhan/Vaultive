using Project.Context;
using Project.DTOs;
using Project.DTOs.MediaContentDTOs;
using Project.DTOs.ReviewDTOs;
using Project.DTOs.StreamingServiceDTOs;
using Project.Models;
using Project.Models.Enumerations;
using Project.Repositories;

namespace Project.Services;

public class MediaContentService
{
    private readonly MediaContentRepository _mediaContentRepository;
    private readonly UserRepository _userRepository;
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;
    private readonly MasterContext _context;

    public MediaContentService(
        MasterContext context,
        MediaContentRepository mediaContentRepository,
        UserRepository userRepository,
        SubscriptionRepository subscriptionRepository,
        WatchHistoryRepository watchHistoryRepository)
    {
        _context = context;
        _mediaContentRepository = mediaContentRepository;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
        _watchHistoryRepository = watchHistoryRepository;
    }

    public async Task<List<MovieResponseDTO>> GetAllMoviesFrontEnd()
    {
        var movies = await _mediaContentRepository.GetAllMovies();
        return movies.Select(m => new MovieResponseDTO
        {
            Genres = m.Genres.Select(g => g.ToString()).ToList(),
            MediaContent = new MediaContentDTO()
            {
                Title = m.Title,
                Description = m.Description,
                YoutubeTrailerURL = m.YoutubeTrailerURL,
                PosterImageName = m.PosterImageName,
                StreamingServices = m.StreamingServices
                    .Select(ss => new StreamingServiceResponseDTO
                    {
                        Name = ss.Name,
                        LogoImage = ss.LogoImage,
                        WebsiteLink = ss.WebsiteLink
                    }).ToList(),
                Reviews = m.Reviews.Select(r => new ReviewResponseDTO()
                {
                    Comment = r.Comment,
                    Nickname = r.User.Nickname,
                }).ToList()

            }
        }).ToList();
        
    }
}