using Project.Context;
using Project.DTOs;
using Project.DTOs.MediaContentDTOs;
using Project.DTOs.ReviewDTOs;
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
    private readonly MasterContext _context;

    public MovieService(
        MasterContext context,
        MovieRepository movieRepository,
        UserRepository userRepository,
        SubscriptionRepository subscriptionRepository,
        WatchHistoryRepository watchHistoryRepository)
    {
        _context = context;
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
                    Name = s.Name,
                    LogoImage = s.LogoImage

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
                StreamingServices = m.StreamingServices
                    .Select(ss => new StreamingServiceResponseDTO
                    {
                        Id = ss.Id,
                        Country = ss.Country,
                        Description = ss.Description,
                        Name = ss.Name,
                        LogoImage = ss.LogoImage
                    }).ToList(),
                Reviews = m.Reviews.Select(r => new ReviewResponseDTO()
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    MediaTitle = r.MediaTitle,
                    Nickname = r.User.Nickname,
                    WatchedOn = r.WatchHistory?.WatchDate.ToString("yyyy-MM-dd")
                }).ToList()

            }
        }).ToList();
        
    }
    public async Task AddOrUpdateWatchHistoryAsync(WatchHistory newHistory)
    {

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existing =
                await _watchHistoryRepository.GetByUserAndMediaAsync(newHistory.UserId, newHistory.MediaTitle);

            if (existing == null)
            {
                await _watchHistoryRepository.AddAsync(newHistory);
            }
            else if (existing.TimeLeftOf != newHistory.TimeLeftOf || existing.WatchDate != newHistory.WatchDate)
            {
                await _watchHistoryRepository.UpdateAsync(existing, newHistory);

            }
            
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
      
        
    }

}