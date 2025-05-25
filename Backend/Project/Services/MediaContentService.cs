using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.DTOs.FrontendDTOs;
using Project.DTOs.MediaContentDTOs;
using Project.DTOs.OptionDTOs;
using Project.DTOs.ReviewDTOs;
using Project.DTOs.StreamingServiceDTOs;
using Project.Exceptions;
using Project.Models;
using Project.Models.Enumerations;
using Project.Repositories;

namespace Project.Services;

public class MediaContentService
{
    private readonly ReviewRepository _reviewRepository;
    private readonly UserRepository _userRepository;
    private readonly StreamingServiceRepository _streamingServiceRepository;
    private readonly SubscriptionConfirmationRepository _subscriptionConfirmationRepository;
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly MediaContentRepository _mediaContentRepository;
    private readonly WatchHistoryRepository _watchHistoryRepository;
    private readonly MasterContext _context;

    public MediaContentService(
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

    /* Adding new movie data to database. */
    public async Task AddMovieAsync(CreateMovieDTO movieDto)
    {
        if (movieDto == null)
            throw new ArgumentNullException(nameof(movieDto));

        if (movieDto.MediaContent == null)
            throw new ArgumentException("MediaContent inside of input can not be null..", nameof(movieDto));

        /* Movie Title must be unique. */
        if (await _context.MediaContents.AnyAsync(m => m.Title == movieDto.MediaContent.Title))
            throw new MediaContentTitleMustBeUniqueException(movieDto.MediaContent.Title);


        /* Before starting the process we are validating if the given genres are parse-able to actual Genre enumeration class. */
        ValidateGenres(movieDto.Genres);
        /* Next step is making sure if there is at least one option is existing since it is a composition-overlapping */
        ValidateOptions(movieDto.MediaContent.AudioOption, movieDto.MediaContent.SubtitleOption);

        ValidateMediaContent(
            title: movieDto.MediaContent.Title,
            description: movieDto.MediaContent.Description,
            originalLanguage: movieDto.MediaContent.OriginalLanguage,
            country: movieDto.MediaContent.Country,
            duration: movieDto.MediaContent.Duration,
            releaseDate: movieDto.MediaContent.ReleaseDate,
            audioLanguages: movieDto.MediaContent.AudioOption?.Languages,
            subtitleLanguages: movieDto.MediaContent.SubtitleOption?.Languages
        );


        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var enums = ParseGenres(movieDto.Genres);

            var streamingServices = await _context.StreamingServices
                .Where(ss => movieDto.MediaContent.StreamingServiceIds.Contains(ss.Id))
                .ToListAsync();

            if (!streamingServices.Any())
                throw new StreamingServiceNotFoundException(movieDto.MediaContent.StreamingServiceIds);

            AudioOption? audioOption = null;
            if (movieDto.MediaContent.AudioOption != null)
            {
                audioOption = new AudioOption
                {
                    Languages = movieDto.MediaContent.AudioOption.Languages
                };
            }

            SubtitleOption? subtitleOption = null;
            if (movieDto.MediaContent.SubtitleOption != null)
            {
                subtitleOption = new SubtitleOption
                {
                    Languages = movieDto.MediaContent.SubtitleOption.Languages
                };
            }


            await _mediaContentRepository.AddAsync(new Movie()
            {
                Title = movieDto.MediaContent.Title,
                AudioOption = audioOption,
                SubtitleOption = subtitleOption,
                Country = movieDto.MediaContent.Country,
                Duration = movieDto.MediaContent.Duration,
                Description = movieDto.MediaContent.Description,
                OriginalLanguage = movieDto.MediaContent.OriginalLanguage,
                ReleaseDate = movieDto.MediaContent.ReleaseDate,
                PosterImageName = movieDto.MediaContent.PosterImageName,
                YoutubeTrailerURL = movieDto.MediaContent.YoutubeTrailerURL,
                Genres = enums,
                StreamingServices = streamingServices
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

    /* Remove the media content with the given id */
    public async Task RemoveMediaContentWithGivenIdAsync(int mediaId)
    {
        if (mediaId <= 0) throw new ArgumentException("Media id can not be equal or smaller than 0.");
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _mediaContentRepository.RemoveAsync(mediaId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /* Update the media content with the given id */
    public async Task UpdateMovieWithGivenIdAsync(int movieId, UpdateMovieDTO movieDto)
    {
        if (movieId <= 0) throw new ArgumentException("Movie id can not be equal or smaller than 0.");

        if (movieDto == null)
            throw new ArgumentNullException(nameof(movieDto));
        if (movieDto.MediaContent == null)
            throw new ArgumentException("MediaContent inside of input can not be null..", nameof(movieDto));

        /* Movie Title must be unique but if the movie trying to update itself, no error is thrown. */
        if (await _context.MediaContents.AnyAsync(m => m.Title == movieDto.MediaContent.Title && m.Id != movieId))
            throw new MediaContentTitleMustBeUniqueException(movieDto.MediaContent.Title);
        /* Next step is making sure if there is at least one option is existing since it is a composition-overlapping */
            ValidateOptions(movieDto.MediaContent.AudioOption, movieDto.MediaContent.SubtitleOption);

        var movie = await _mediaContentRepository.GetMovieWithGivenIdAsync(movieId);
        if (movie == null) throw new MediaContentDoesNotExistsException(movieId);

        ValidateMediaContent(
            title: movieDto.MediaContent.Title,
            description: movieDto.MediaContent.Description,
            originalLanguage: movieDto.MediaContent.OriginalLanguage,
            country: movieDto.MediaContent.Country,
            duration: movieDto.MediaContent.Duration,
            releaseDate: movieDto.MediaContent.ReleaseDate,
            audioLanguages: movieDto.MediaContent.AudioOption?.Languages,
            subtitleLanguages: movieDto.MediaContent.SubtitleOption?.Languages
        );


        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            movie.Title = movieDto.MediaContent.Title;
            movie.Description = movieDto.MediaContent.Description;
            movie.ReleaseDate = movieDto.MediaContent.ReleaseDate;
            movie.OriginalLanguage = movieDto.MediaContent.OriginalLanguage;
            movie.Country = movieDto.MediaContent.Country;
            movie.Duration = movieDto.MediaContent.Duration;
            movie.PosterImageName = movieDto.MediaContent.PosterImageName;
            movie.YoutubeTrailerURL = movieDto.MediaContent.YoutubeTrailerURL;

            if (movie.AudioOption != null && movieDto.MediaContent.AudioOption != null)
            {
                movie.AudioOption.Languages = movieDto.MediaContent.AudioOption.Languages;
            }
            else if (movie.AudioOption == null && movieDto.MediaContent.AudioOption != null)
            {
                movie.AudioOption = new AudioOption()
                {
                    MediaContent = movie,
                    Languages = movieDto.MediaContent.AudioOption.Languages
                };
            } else if (movie.AudioOption != null && movieDto.MediaContent.AudioOption == null)
            {
                movie.AudioOption = null;
            }

            if (movie.SubtitleOption != null && movieDto.MediaContent.SubtitleOption != null)
            {
                movie.SubtitleOption.Languages = movieDto.MediaContent.SubtitleOption.Languages;
            }
            else if (movie.SubtitleOption == null && movieDto.MediaContent.SubtitleOption != null)
            {
                movie.SubtitleOption = new SubtitleOption()
                {
                    MediaContent = movie,
                    Languages = movieDto.MediaContent.SubtitleOption.Languages
                };
            }else if (movie.SubtitleOption != null && movieDto.MediaContent.SubtitleOption == null)
            {
                movie.SubtitleOption = null;
            }

            var existingIds = movie.StreamingServices
                .Select(s => s.Id)
                .ToHashSet();
            var desiredIds = movieDto.MediaContent.StreamingServiceIds;

            var toRemoveStreamingServices = movie.StreamingServices
                .Where(s => !desiredIds.Contains(s.Id))
                .ToList();
            foreach (var svc in toRemoveStreamingServices)
                movie.StreamingServices.Remove(svc);

            var toAddIds = desiredIds.Except(existingIds).ToList();
            if (toAddIds.Any())
            {
                var toAddStreamingServices = await _context.StreamingServices
                    .Where(s => toAddIds.Contains(s.Id))
                    .ToListAsync();
                foreach (var svc in toAddStreamingServices)
                    movie.StreamingServices.Add(svc);
            }


            var desiredGenres = ParseGenres(movieDto.Genres);

            var existingGenres = movie.Genres.ToList();

            var toRemoveGenres = existingGenres
                .Where(g => !desiredGenres.Contains(g))
                .ToList();
            foreach (var g in toRemoveGenres)
                movie.Genres.Remove(g);

            var toAddGenres = desiredGenres
                .Where(g => !existingGenres.Contains(g))
                .ToList();
            foreach (var g in toAddGenres)
                movie.Genres.Add(g);


            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    /* Returns all the movies with their reviews and streaming services. */
    public async Task<List<MovieResponseFrontendDTO>> GetAllMoviesFrontEndAsync()
    {
        var movies = await _mediaContentRepository.GetAllMoviesAsync();
        return movies.Select(m => new MovieResponseFrontendDTO
        {
            MediaContent = new MediaContentFrontendResponseDTO()
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                YoutubeTrailerURL = m.YoutubeTrailerURL,
                PosterImageName = m.PosterImageName,
                StreamingServices = m.StreamingServices
                    .Select(ss => new StreamingServiceResponseFrontendDTO()
                    {
                        Name = ss.Name,
                        LogoImage = ss.LogoImage,
                        WebsiteLink = ss.WebsiteLink
                    }).ToList(),
                Reviews = m.Reviews.Select(r => new ReviewResponseFrontendDTO()
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    Nickname = r.User.Nickname,
                }).ToList()
            },
            Genres = m.Genres.Select(g => g.ToString()).ToList()
        }).ToList();
    }


    /* Main difference than the GetAllMoviesFrontEndAsync is returning all details like Duration, Ids etc. */
    public async Task<List<MovieDTO>> GetAllMoviesDetailedAsync()
    {
        var movies = await _mediaContentRepository.GetAllMoviesAsync();

        return movies.Select(m => new MovieDTO
        {
            MediaContentDetailed = new MediaContentDetailedDTO()
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                YoutubeTrailerURL = m.YoutubeTrailerURL,
                PosterImageName = m.PosterImageName,
                Country = m.Country,
                Duration = m.Duration,
                OriginalLanguage = m.OriginalLanguage,
                ReleaseDate = m.ReleaseDate,
                StreamingServices = m.StreamingServices
                    .Select(ss => new StreamingServiceDTO
                    {
                        Id = ss.Id,
                        Country = ss.Country,
                        DefaultPrice = ss.DefaultPrice,
                        Description = ss.Description,
                        Name = ss.Name,
                        LogoImage = ss.LogoImage,
                        WebsiteLink = ss.WebsiteLink
                    }).ToList(),
                Reviews = m.Reviews.Select(r => new ReviewDTO()
                {
                    Id = r.Id,
                    MediaTitle = r.MediaContent.Title,
                    WatchedOn = r.WatchHistory.WatchDate,
                    Comment = r.Comment,
                    Nickname = r.User.Nickname,
                }).ToList(),
                AudioOption = new OptionDTO()
                {
                    Languages = m.AudioOption?.Languages
                },
                SubtitleOption = new OptionDTO()
                {
                    Languages = m.SubtitleOption?.Languages
                }
            },
            Genres = m.Genres.Select(g => g.ToString()).ToList()
        }).ToList();
    }

    /* Get one media content by id including all details, with given id */
    public async Task<MediaContentDetailedDTO> GetMediaContentWithGivenIdAsync(int mediaId)
    {
        if (mediaId <= 0) throw new ArgumentException("Media id can not be equal or smaller than 0.");

        var mediaContent = await _mediaContentRepository.GetMediaContentWithGivenIdAsync(mediaId);
        if (mediaContent == null) throw new MediaContentDoesNotExistsException(mediaId);

        return new MediaContentDetailedDTO
        {
            Id = mediaContent.Id,
            Title = mediaContent.Title,
            Description = mediaContent.Description,
            YoutubeTrailerURL = mediaContent.YoutubeTrailerURL,
            PosterImageName = mediaContent.PosterImageName,
            Country = mediaContent.Country,
            Duration = mediaContent.Duration,
            OriginalLanguage = mediaContent.OriginalLanguage,
            StreamingServices = mediaContent.StreamingServices
                .Select(ss => new StreamingServiceDTO
                {
                    Id = ss.Id,
                    Country = ss.Country,
                    DefaultPrice = ss.DefaultPrice,
                    Description = ss.Description,
                    Name = ss.Name,
                    LogoImage = ss.LogoImage,
                    WebsiteLink = ss.WebsiteLink
                }).ToList(),
            Reviews = mediaContent.Reviews.Select(r => new ReviewDTO()
            {
                Id = r.Id,
                MediaTitle = r.MediaContent.Title,
                WatchedOn = r.WatchHistory.WatchDate,
                Comment = r.Comment,
                Nickname = r.User.Nickname,
            }).ToList(),
            AudioOption = new OptionDTO()
            {
                Languages = mediaContent.AudioOption?.Languages
            },
            SubtitleOption = new OptionDTO()
            {
                Languages = mediaContent.SubtitleOption?.Languages
            }
        };
    }

    /* Get one movie by id including all details, with given id */
    public async Task<MovieDTO> GetMovieWithGivenIdAsync(int movieId)
    {
        if (movieId <= 0) throw new ArgumentException("Movie id can not be equal or smaller than 0.");

        var movie = await _mediaContentRepository.GetMovieWithGivenIdAsync(movieId);
        if (movie == null) throw new MovieNotFoundException(movieId);

        return new MovieDTO
        {
            MediaContentDetailed = new MediaContentDetailedDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                YoutubeTrailerURL = movie.YoutubeTrailerURL,
                PosterImageName = movie.PosterImageName,
                Country = movie.Country,
                Duration = movie.Duration,
                OriginalLanguage = movie.OriginalLanguage,
                ReleaseDate = movie.ReleaseDate,
                StreamingServices = movie.StreamingServices
                    .Select(ss => new StreamingServiceDTO
                    {
                        Id = ss.Id,
                        Country = ss.Country,
                        DefaultPrice = ss.DefaultPrice,
                        Description = ss.Description,
                        Name = ss.Name,
                        LogoImage = ss.LogoImage,
                        WebsiteLink = ss.WebsiteLink
                    }).ToList(),
                Reviews = movie.Reviews.Select(r => new ReviewDTO()
                {
                    Id = r.Id,
                    MediaTitle = r.MediaContent.Title,
                    WatchedOn = r.WatchHistory.WatchDate,
                    Comment = r.Comment,
                    Nickname = r.User.Nickname,
                }).ToList(),
                AudioOption = new OptionDTO()
                {
                    Languages = movie.AudioOption?.Languages
                },
                SubtitleOption = new OptionDTO()
                {
                    Languages = movie.SubtitleOption?.Languages
                }
            },
            Genres = movie.Genres.Select(g => g.ToString()).ToList()
        };
    }


    private void ValidateGenres(ICollection<string> genres)
    {
        if (genres == null || !genres.Any())
            throw new AtLeastOneGenreMustExistsException();
    }

    private void ValidateOptions(OptionDTO? audioOption, OptionDTO? subtitleOption)
    {
        if (audioOption == null && subtitleOption == null)
            throw new AtLeastOneOptionMustExistsException();
    }

    private static HashSet<Genre> ParseGenres(IEnumerable<string> genres)
    {
        return genres
            .Select(g => Enum.TryParse<Genre>(g, true, out var result)
                ? result
                : throw new InvalidGenreException(g))
            .ToHashSet();
    }

    private void ValidateMediaContent(
        string title,
        string description,
        string originalLanguage,
        string country,
        int duration,
        DateOnly releaseDate,
        ICollection<string>? audioLanguages = null,
        ICollection<string>? subtitleLanguages = null)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < 2 || title.Length > 100)
            throw new ArgumentException("Title must be 2–100 characters.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description must not be empty.", nameof(description));

        if (string.IsNullOrWhiteSpace(originalLanguage))
            throw new ArgumentException("Original language is required.", nameof(originalLanguage));

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required.", nameof(country));

        if (duration <= 0)
            throw new ArgumentException("Duration must be positive.", nameof(duration));

        if (releaseDate == default)
            throw new ArgumentException("Valid release date required.", nameof(releaseDate));

        if (audioLanguages != null && !audioLanguages.Any())
            throw new ArgumentException("At least one audio language is required.", nameof(audioLanguages));

        if (subtitleLanguages != null && !subtitleLanguages.Any())
            throw new ArgumentException("At least one subtitle language is required.", nameof(subtitleLanguages));
    }
}