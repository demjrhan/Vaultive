using Project.DTOs;
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

    /*public async Task<MovieResponseDTO> GetMoviesWithGivenGenre(Genre genre)
    {
        var movies = await _movieRepository.GetMoviesWithGivenGenre(genre);
        movies.Select(m => new MovieResponseDTO
        {
            
        })
    }*/
}