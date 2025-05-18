using Project.DTOs;
using Project.DTOs.MediaContentDTOs;
using Project.DTOs.StreamingServiceDTOs;
using Project.Models;
using Project.Models.Enumerations;
using Project.Repositories;

namespace Project.Services;

public class MovieService
{
    private readonly MovieRepository _movieRepository;
    private readonly UserRepository _userRepository;
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;

    public MovieService(MovieRepository movieRepository,
        UserRepository userRepository,
        SubscriptionRepository subscriptionRepository,
        WatchHistoryRepository watchHistoryRepository)
    {
        _movieRepository = movieRepository;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
        _watchHistoryRepository = watchHistoryRepository;
    }

    public async Task<List<MovieResponseDTO>> GetMoviesWithGivenGenre(Genre genre)
    {
        var movies = await _movieRepository.GetMoviesWithGivenGenre(genre);
        return movies.Select(m => new MovieResponseDTO
        {
            Genres = m.Genres.Select(g => g.ToString()).ToList(),
            MediaContent = new MediaContentResponseDTO()
            {
                Country = m.Country,
                Description = m.Description,
                Duration = m.Duration,
                OriginalLanguage = m.OriginalLanguage,
                ReleaseDate = m.ReleaseDate,
                Title = m.Title,
                YoutubeTrailerURL = m.YoutubeTrailerURL,
                PosterImageName = m.PosterImageName,
                StreamingServices = m.StreamingServices.Select(s => new StreamingServiceResponseDTO()
                {
                    Country = s.Country,
                    Description = s.Description,
                    Name = s.Name
                }).ToList()
            }
        }).ToList();
    }
    public async Task<List<MovieResponseDTO>> GetAllMovies()
    {
        var movies = await _movieRepository.GetAllMovies();
        return movies.Select(m => new MovieResponseDTO
        {
            Genres = m.Genres.Select(g => g.ToString()).ToList(),
            MediaContent = new MediaContentResponseDTO()
            {
                Country = m.Country,
                Description = m.Description,
                Duration = m.Duration,
                OriginalLanguage = m.OriginalLanguage,
                ReleaseDate = m.ReleaseDate,
                Title = m.Title,
                YoutubeTrailerURL = m.YoutubeTrailerURL,
                PosterImageName = m.PosterImageName,
                StreamingServices = m.MediaContentStreamingServices
                    .Select(mcs => new StreamingServiceResponseDTO
                    {
                        Country = mcs.StreamingService.Country,
                        Description = mcs.StreamingService.Description,
                        Name = mcs.StreamingService.Name,
                    }).ToList()

            }
        }).ToList();
        
    }
    public async Task AddOrUpdateWatchHistoryAsync(WatchHistory newHistory)
    {
        var existing = await _watchHistoryRepository.GetByUserAndMediaAsync(newHistory.UserId, newHistory.MediaTitle);

        if (existing == null)
        {
            await _watchHistoryRepository.AddAsync(newHistory);
            await _watchHistoryRepository.SaveChangesAsync();
        }
        else if (existing.TimeLeftOf != newHistory.TimeLeftOf || existing.WatchDate != newHistory.WatchDate)
        {
            await _watchHistoryRepository.UpdateAsync(existing, newHistory);
            await _watchHistoryRepository.SaveChangesAsync();

        }
        
    }

}