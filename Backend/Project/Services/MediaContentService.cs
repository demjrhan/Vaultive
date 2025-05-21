using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.Context;
using Project.DTOs;
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

    /* Adding new movie data to database. */
    public async Task AddMovieAsync(CreateMovieDTO movieDto)
    {
        /* Before starting the process we are validating if the given genres are parse-able to actual Genre enumeration class. */
        ValidateGenres(movieDto.Genres);
        /* Next step is making sure if there is at least one option is existing since it is a composition-overlapping */
        ValidateOptions(movieDto.MediaContent.AudioOption, movieDto.MediaContent.SubtitleOption);


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
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new AddDataFailedException(ex);
        }
    }


    /* Returns all the movies with their reviews and streaming services. */
    public async Task<List<MovieResponseFrontendDTO>> GetAllMoviesFrontEndAsync()
    {
        var movies = await _mediaContentRepository.GetAllMovies();
        return movies.Select(m => new MovieResponseFrontendDTO
        {
            Genres = m.Genres.Select(g => g.ToString()).ToList(),
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
            }
        }).ToList();
    }


    /* Main difference than the GetAllMoviesFrontEndAsync is returning all details like Duration, Ids etc. */
    public async Task<List<MovieResponseDTO>> GetAllMoviesDetailedAsync()
    {
        var movies = await _mediaContentRepository.GetAllMovies();

        return movies.Select(m => new MovieResponseDTO
        {
            Genres = m.Genres.Select(g => g.ToString()).ToList(),
            MediaContentResponse = new MediaContentResponseDTO()
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
                    .Select(ss => new StreamingServiceResponseDTO
                    {
                        Id = ss.Id,
                        Country = ss.Country,
                        DefaultPrice = ss.DefaultPrice,
                        Description = ss.Description,
                        Name = ss.Name,
                        LogoImage = ss.LogoImage,
                        WebsiteLink = ss.WebsiteLink
                    }).ToList(),
                Reviews = m.Reviews.Select(r => new ReviewResponseDTO()
                {
                    Id = r.Id,
                    MediaTitle = r.MediaContent.Title,
                    WatchedOn = r.WatchHistory.WatchDate.ToShortDateString(),
                    Comment = r.Comment,
                    Nickname = r.User.Nickname,
                }).ToList()
            }
        }).ToList();
    }

    /* Get one media content including all details, with given id */
    public async Task<MediaContentResponseDTO> GetMediaContentWithGivenIdAsync(int mediaId)
    {
        var mediaContent = await _mediaContentRepository.GetMediaContentWithGivenIdAsync(mediaId);
        if (mediaContent == null) throw new MediaContentDoesNotExistsException(mediaId);

        return new MediaContentResponseDTO
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
                .Select(ss => new StreamingServiceResponseDTO
                {
                    Id = ss.Id,
                    Country = ss.Country,
                    DefaultPrice = ss.DefaultPrice,
                    Description = ss.Description,
                    Name = ss.Name,
                    LogoImage = ss.LogoImage,
                    WebsiteLink = ss.WebsiteLink
                }).ToList(),
            Reviews = mediaContent.Reviews.Select(r => new ReviewResponseDTO()
            {
                Id = r.Id,
                MediaTitle = r.MediaContent.Title,
                WatchedOn = r.WatchHistory.WatchDate.ToShortDateString(),
                Comment = r.Comment,
                Nickname = r.User.Nickname,
            }).ToList()
        };
    }


    /* Remove the media content with the given id */
    public async Task RemoveMediaContentWithGivenIdAsync(int mediaId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _mediaContentRepository.RemoveAsync(mediaId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new RemoveDataFailedException(ex);
        }
    }


    public async Task UpdateMovieWithGivenIdAsync(int movieId, UpdateMovieDTO movieDto)
    {
        var mediaContent = await _mediaContentRepository.GetMovieWithGivenIdAsync(movieId);
        if (mediaContent == null) throw new MediaContentDoesNotExistsException(movieId);

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            mediaContent.Title = movieDto.MediaContent.Title;
            mediaContent.Description = movieDto.MediaContent.Description;
            mediaContent.ReleaseDate = movieDto.MediaContent.ReleaseDate;
            mediaContent.OriginalLanguage = movieDto.MediaContent.OriginalLanguage;
            mediaContent.Country = movieDto.MediaContent.Country;
            mediaContent.Duration = movieDto.MediaContent.Duration;
            mediaContent.PosterImageName = movieDto.MediaContent.PosterImageName;
            mediaContent.YoutubeTrailerURL = movieDto.MediaContent.YoutubeTrailerURL;

            if (mediaContent.AudioOption != null && movieDto.MediaContent.AudioOption != null)
            {
                mediaContent.AudioOption.Languages = movieDto.MediaContent.AudioOption.Languages;
            }
            else if (mediaContent.AudioOption == null && movieDto.MediaContent.AudioOption != null)
            {
                mediaContent.AudioOption = new AudioOption()
                {
                    Languages = movieDto.MediaContent.AudioOption.Languages
                };
            }

            if (mediaContent.SubtitleOption != null && movieDto.MediaContent.SubtitleOption != null)
            {
                mediaContent.SubtitleOption.Languages = movieDto.MediaContent.SubtitleOption.Languages;
            }
            else if (mediaContent.SubtitleOption == null && movieDto.MediaContent.SubtitleOption != null)
            {
                mediaContent.SubtitleOption = new SubtitleOption()
                {
                    Languages = movieDto.MediaContent.SubtitleOption.Languages
                };
            }

            var existingIds = mediaContent.StreamingServices
                .Select(s => s.Id)
                .ToHashSet();
            var desiredIds = movieDto.MediaContent.StreamingServiceIds;

            var toRemoveStreamingServices = mediaContent.StreamingServices
                .Where(s => !desiredIds.Contains(s.Id))
                .ToList();
            foreach (var svc in toRemoveStreamingServices)
                mediaContent.StreamingServices.Remove(svc);

            var toAddIds = desiredIds.Except(existingIds).ToList();
            if (toAddIds.Any())
            {
                var toAddStreamingServices = await _context.StreamingServices
                    .Where(s => toAddIds.Contains(s.Id))
                    .ToListAsync();
                foreach (var svc in toAddStreamingServices)
                    mediaContent.StreamingServices.Add(svc);
            }


            var desiredGenres = ParseGenres(movieDto.Genres);

            var existingGenres = mediaContent.Genres.ToList();

            var toRemoveGenres = existingGenres
                .Where(g => !desiredGenres.Contains(g))
                .ToList();
            foreach (var g in toRemoveGenres)
                mediaContent.Genres.Remove(g);

            var toAddGenres = desiredGenres
                .Where(g => !existingGenres.Contains(g))
                .ToList();
            foreach (var g in toAddGenres)
                mediaContent.Genres.Add(g);


            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new UpdateDataFailedException(ex);
        }
    }

    private void ValidateGenres(List<string> genres)
    {
        if (genres == null || !genres.Any())
            throw new AtLeastOneGenreMustExistsException();
    }

    private void ValidateOptions(OptionDTO? audioOption, OptionDTO? subtitleOption)
    {
        if (audioOption == null && subtitleOption == null)
            throw new AtLeastOneOptionMustExistsException();
    }

    private static List<Genre> ParseGenres(IEnumerable<string> genres)
    {
        return genres
            .Select(g => Enum.TryParse<Genre>(g, true, out var result)
                ? result
                : throw new InvalidGenreException(g))
            .ToList();
    }
}