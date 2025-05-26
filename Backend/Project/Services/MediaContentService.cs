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
    public async Task AddMovieAsync(CreateMovieDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.MediaContent == null)
            throw new ArgumentException("MediaContent inside of input can not be null..", nameof(dto));

        /* Movie Title must be unique. */
        if (await _context.MediaContents.AnyAsync(m => m.Title == dto.MediaContent.Title))
            throw new MediaContentTitleMustBeUniqueException(dto.MediaContent.Title);


        /* Before starting the process we are validating if the given genres are parse-able to actual Genre enumeration class. */
        ValidateGenres(dto.Genres);
        /* Next step is making sure if there is at least one option is existing since it is a composition-overlapping */
        ValidateOptions(dto.MediaContent.AudioOption, dto.MediaContent.SubtitleOption);

        ValidateMediaContent(
            title: dto.MediaContent.Title,
            description: dto.MediaContent.Description,
            originalLanguage: dto.MediaContent.OriginalLanguage,
            country: dto.MediaContent.Country,
            duration: dto.MediaContent.Duration,
            releaseDate: dto.MediaContent.ReleaseDate,
            audioLanguages: dto.MediaContent.AudioOption?.Languages,
            subtitleLanguages: dto.MediaContent.SubtitleOption?.Languages
        );


        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var enums = ParseGenres(dto.Genres);
            List<StreamingService> streamingServices = new List<StreamingService>();
            if (dto.MediaContent.StreamingServiceIds.Any())
            {
                if (dto.MediaContent.StreamingServiceIds.Any(id => id <= 0))
                    throw new ArgumentException(
                        "All streaming service IDs must be positive integers.",
                        nameof(dto.MediaContent.StreamingServiceIds));

                var requestedIds = dto.MediaContent.StreamingServiceIds.Distinct().ToList();

                streamingServices = await _context.StreamingServices
                    .Where(ss => requestedIds.Contains(ss.Id))
                    .ToListAsync();

                var foundIds = streamingServices.Select(ss => ss.Id);
                var missingIds = requestedIds.Except(foundIds).ToList();

                if (missingIds.Any())
                    throw new StreamingServiceDoesNotExistsException(missingIds);
            }

            AudioOption? audioOption = null;
            if (dto.MediaContent.AudioOption != null)
            {
                audioOption = new AudioOption
                {
                    Languages = dto.MediaContent.AudioOption.Languages
                };
            }

            SubtitleOption? subtitleOption = null;
            if (dto.MediaContent.SubtitleOption != null)
            {
                subtitleOption = new SubtitleOption
                {
                    Languages = dto.MediaContent.SubtitleOption.Languages
                };
            }


            await _mediaContentRepository.AddAsync(new Movie()
            {
                Title = dto.MediaContent.Title,
                AudioOption = audioOption,
                SubtitleOption = subtitleOption,
                Country = dto.MediaContent.Country,
                Duration = dto.MediaContent.Duration,
                Description = dto.MediaContent.Description,
                OriginalLanguage = dto.MediaContent.OriginalLanguage,
                ReleaseDate = dto.MediaContent.ReleaseDate,
                PosterImageName = dto.MediaContent.PosterImageName,
                YoutubeTrailerURL = dto.MediaContent.YoutubeTrailerURL,
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

    /* Adding new documentary data to database. */

    public async Task AddDocumentaryAsync(CreateDocumentaryDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.MediaContent == null)
            throw new ArgumentException("MediaContent inside of input cannot be null.", nameof(dto));

        /* Documentary Title must be unique. */
        if (await _context.MediaContents.AnyAsync(m => m.Title == dto.MediaContent.Title))
            throw new MediaContentTitleMustBeUniqueException(dto.MediaContent.Title);

        /* Before starting the process we are validating if the given genres are parse-able to actual Genre enumeration class. */
        ValidateTopics(dto.Topics);

        /* Next step is making sure if there is at least one option is existing since it is a composition-overlapping */
        ValidateOptions(dto.MediaContent.AudioOption, dto.MediaContent.SubtitleOption);

        ValidateMediaContent(
            title: dto.MediaContent.Title,
            description: dto.MediaContent.Description,
            originalLanguage: dto.MediaContent.OriginalLanguage,
            country: dto.MediaContent.Country,
            duration: dto.MediaContent.Duration,
            releaseDate: dto.MediaContent.ReleaseDate,
            audioLanguages: dto.MediaContent.AudioOption?.Languages,
            subtitleLanguages: dto.MediaContent.SubtitleOption?.Languages
        );

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var topicEnums = ParseTopics(dto.Topics);

            List<StreamingService> streamingServices = new List<StreamingService>();
            if (dto.MediaContent.StreamingServiceIds.Any())
            {
                if (dto.MediaContent.StreamingServiceIds.Any(id => id <= 0))
                    throw new ArgumentException(
                        "All streaming service IDs must be positive integers.",
                        nameof(dto.MediaContent.StreamingServiceIds));

                var requestedIds = dto.MediaContent.StreamingServiceIds.Distinct().ToList();
                streamingServices = await _context.StreamingServices
                    .Where(ss => requestedIds.Contains(ss.Id))
                    .ToListAsync();

                var foundIds = streamingServices.Select(ss => ss.Id);
                var missingIds = requestedIds.Except(foundIds).ToList();
                if (missingIds.Any())
                    throw new StreamingServiceDoesNotExistsException(missingIds);
            }

            AudioOption? audioOption = null;
            if (dto.MediaContent.AudioOption != null)
            {
                audioOption = new AudioOption
                {
                    Languages = dto.MediaContent.AudioOption.Languages
                };
            }

            SubtitleOption? subtitleOption = null;
            if (dto.MediaContent.SubtitleOption != null)
            {
                subtitleOption = new SubtitleOption
                {
                    Languages = dto.MediaContent.SubtitleOption.Languages
                };
            }

            var documentary = new Documentary
            {
                Title = dto.MediaContent.Title,
                Description = dto.MediaContent.Description,
                OriginalLanguage = dto.MediaContent.OriginalLanguage,
                Country = dto.MediaContent.Country,
                Duration = dto.MediaContent.Duration,
                ReleaseDate = dto.MediaContent.ReleaseDate,
                PosterImageName = dto.MediaContent.PosterImageName,
                YoutubeTrailerURL = dto.MediaContent.YoutubeTrailerURL,
                AudioOption = audioOption,
                SubtitleOption = subtitleOption,
                Topics = topicEnums,
                StreamingServices = streamingServices
            };

            await _mediaContentRepository.AddAsync(documentary);
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
    public async Task UpdateMovieWithGivenIdAsync(int movieId, UpdateMovieDTO dto)
    {
        if (movieId <= 0) throw new ArgumentException("Movie id can not be equal or smaller than 0.");

        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        if (dto.MediaContent == null)
            throw new ArgumentException("MediaContent inside of input can not be null..", nameof(dto));

        var movie = await _mediaContentRepository.GetMovieWithGivenIdAsync(movieId);
        if (movie == null) throw new MediaContentDoesNotExistsException(new[] { movieId });

        ValidateChanges(dto, movie);
        
        ValidateGenres(dto.Genres);

        /* Movie Title must be unique but if the movie trying to update itself, no error is thrown. */
        if (await _context.MediaContents.AnyAsync(m => m.Title == dto.MediaContent.Title && m.Id != movieId))
            throw new MediaContentTitleMustBeUniqueException(dto.MediaContent.Title);
        /* Next step is making sure if there is at least one option is existing since it is a composition-overlapping */
        ValidateOptions(dto.MediaContent.AudioOption, dto.MediaContent.SubtitleOption);


        ValidateMediaContent(
            title: dto.MediaContent.Title,
            description: dto.MediaContent.Description,
            originalLanguage: dto.MediaContent.OriginalLanguage,
            country: dto.MediaContent.Country,
            duration: dto.MediaContent.Duration,
            releaseDate: dto.MediaContent.ReleaseDate,
            audioLanguages: dto.MediaContent.AudioOption?.Languages,
            subtitleLanguages: dto.MediaContent.SubtitleOption?.Languages
        );


        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            movie.Title = dto.MediaContent.Title;
            movie.Description = dto.MediaContent.Description;
            movie.ReleaseDate = dto.MediaContent.ReleaseDate;
            movie.OriginalLanguage = dto.MediaContent.OriginalLanguage;
            movie.Country = dto.MediaContent.Country;
            movie.Duration = dto.MediaContent.Duration;
            movie.PosterImageName = dto.MediaContent.PosterImageName;
            movie.YoutubeTrailerURL = dto.MediaContent.YoutubeTrailerURL;

            if (movie.AudioOption != null && dto.MediaContent.AudioOption != null)
            {
                movie.AudioOption.Languages = dto.MediaContent.AudioOption.Languages;
            }
            else if (movie.AudioOption == null && dto.MediaContent.AudioOption != null)
            {
                movie.AudioOption = new AudioOption()
                {
                    MediaContent = movie,
                    Languages = dto.MediaContent.AudioOption.Languages
                };
            }
            else if (movie.AudioOption != null && dto.MediaContent.AudioOption == null)
            {
                movie.AudioOption = null;
            }

            if (movie.SubtitleOption != null && dto.MediaContent.SubtitleOption != null)
            {
                movie.SubtitleOption.Languages = dto.MediaContent.SubtitleOption.Languages;
            }
            else if (movie.SubtitleOption == null && dto.MediaContent.SubtitleOption != null)
            {
                movie.SubtitleOption = new SubtitleOption()
                {
                    MediaContent = movie,
                    Languages = dto.MediaContent.SubtitleOption.Languages
                };
            }
            else if (movie.SubtitleOption != null && dto.MediaContent.SubtitleOption == null)
            {
                movie.SubtitleOption = null;
            }

            var existingIds = movie.StreamingServices
                .Select(s => s.Id)
                .ToHashSet();
            var desiredIds = dto.MediaContent.StreamingServiceIds;

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

                var foundIds = toAddStreamingServices.Select(ss => ss.Id);
                var missingIds = toAddIds.Except(foundIds).ToList();

                if (missingIds.Any())
                    throw new StreamingServiceDoesNotExistsException(missingIds);

                foreach (var svc in toAddStreamingServices)
                    movie.StreamingServices.Add(svc);
            }


            var desiredGenres = ParseGenres(dto.Genres);

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

    /* Update the documentary content with the given id */
    public async Task UpdateDocumentaryWithGivenIdAsync(int documentaryId, UpdateDocumentaryDTO dto)
    {
        if (documentaryId <= 0)
            throw new ArgumentException("Documentary id cannot be equal to or less than 0.", nameof(documentaryId));

        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        if (dto.MediaContent == null)
            throw new ArgumentException("MediaContent inside of input cannot be null.", nameof(dto));

        var documentary = await _mediaContentRepository.GetDocumentaryWithGivenIdAsync(documentaryId);
        if (documentary == null)
            throw new MediaContentDoesNotExistsException(new[] { documentaryId });

        ValidateDocumentaryChanges(dto, documentary);

        if (await _context.MediaContents.AnyAsync(m =>
                m.Title == dto.MediaContent.Title && m.Id != documentaryId))
        {
            throw new MediaContentTitleMustBeUniqueException(dto.MediaContent.Title);
        }
        
        ValidateTopics(dto.Topics);

        ValidateOptions(dto.MediaContent.AudioOption, dto.MediaContent.SubtitleOption);

        ValidateMediaContent(
            title: dto.MediaContent.Title,
            description: dto.MediaContent.Description,
            originalLanguage: dto.MediaContent.OriginalLanguage,
            country: dto.MediaContent.Country,
            duration: dto.MediaContent.Duration,
            releaseDate: dto.MediaContent.ReleaseDate,
            audioLanguages: dto.MediaContent.AudioOption?.Languages,
            subtitleLanguages: dto.MediaContent.SubtitleOption?.Languages
        );

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            documentary.Title = dto.MediaContent.Title;
            documentary.Description = dto.MediaContent.Description;
            documentary.ReleaseDate = dto.MediaContent.ReleaseDate;
            documentary.OriginalLanguage = dto.MediaContent.OriginalLanguage;
            documentary.Country = dto.MediaContent.Country;
            documentary.Duration = dto.MediaContent.Duration;
            documentary.PosterImageName = dto.MediaContent.PosterImageName;
            documentary.YoutubeTrailerURL = dto.MediaContent.YoutubeTrailerURL;

            if (documentary.AudioOption != null && dto.MediaContent.AudioOption != null)
            {
                documentary.AudioOption.Languages = dto.MediaContent.AudioOption.Languages;
            }
            else if (documentary.AudioOption == null && dto.MediaContent.AudioOption != null)
            {
                documentary.AudioOption = new AudioOption
                {
                    MediaContent = documentary,
                    Languages = dto.MediaContent.AudioOption.Languages
                };
            }
            else if (documentary.AudioOption != null && dto.MediaContent.AudioOption == null)
            {
                documentary.AudioOption = null;
            }

            if (documentary.SubtitleOption != null && dto.MediaContent.SubtitleOption != null)
            {
                documentary.SubtitleOption.Languages = dto.MediaContent.SubtitleOption.Languages;
            }
            else if (documentary.SubtitleOption == null && dto.MediaContent.SubtitleOption != null)
            {
                documentary.SubtitleOption = new SubtitleOption
                {
                    MediaContent = documentary,
                    Languages = dto.MediaContent.SubtitleOption.Languages
                };
            }
            else if (documentary.SubtitleOption != null && dto.MediaContent.SubtitleOption == null)
            {
                documentary.SubtitleOption = null;
            }
            
            
            var existingIds = documentary.StreamingServices
                .Select(s => s.Id)
                .ToHashSet();
            var desiredIds = dto.MediaContent.StreamingServiceIds;

            var toRemoveStreamingServices = documentary.StreamingServices
                .Where(s => !desiredIds.Contains(s.Id))
                .ToList();
            foreach (var svc in toRemoveStreamingServices)
                documentary.StreamingServices.Remove(svc);
            
            var toAddIds = desiredIds.Except(existingIds).ToList();
            if (toAddIds.Any())
            {
                var toAddStreamingServices = await _context.StreamingServices
                    .Where(s => toAddIds.Contains(s.Id))
                    .ToListAsync();

                var foundIds = toAddStreamingServices.Select(ss => ss.Id);
                var missingIds = toAddIds.Except(foundIds).ToList();

                if (missingIds.Any())
                    throw new StreamingServiceDoesNotExistsException(missingIds);

                foreach (var svc in toAddStreamingServices)
                    documentary.StreamingServices.Add(svc);
            }
            var desiredTopics = ParseTopics(dto.Topics);
            var existingTopics = documentary.Topics.ToList();

            var toRemoveTopics = existingTopics
                .Where(t => !desiredTopics.Contains(t))
                .ToList();
            foreach (var t in toRemoveTopics)
                documentary.Topics.Remove(t);

            var toAddTopics = desiredTopics
                .Where(t => !existingTopics.Contains(t))
                .ToList();
            foreach (var t in toAddTopics)
                documentary.Topics.Add(t);
            
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
                        WebsiteLink = ss.WebsiteLink,
                        LogoImage = ss.LogoImage
                    }).ToList(),
                Reviews = m.Reviews.Select(r => new ReviewResponseFrontendDTO()
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    Nickname = r.User.Nickname,
                    WatchedOn = r.WatchHistory.WatchDate,
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

    /* Returns all the documentaries with their reviews and streaming services with all included details. */

    public async Task<List<DocumentaryDTO>> GetAllDocumentariesDetailedAsync()
    {
        var docs = await _mediaContentRepository.GetAllDocumentariesAsync();

        return docs.Select(d => new DocumentaryDTO
        {
            MediaContentDetailed = new MediaContentDetailedDTO
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                YoutubeTrailerURL = d.YoutubeTrailerURL,
                PosterImageName = d.PosterImageName,
                Country = d.Country,
                Duration = d.Duration,
                OriginalLanguage = d.OriginalLanguage,
                ReleaseDate = d.ReleaseDate,
                StreamingServices = d.StreamingServices
                    .Select(ss => new StreamingServiceDTO
                    {
                        Id = ss.Id,
                        Country = ss.Country,
                        DefaultPrice = ss.DefaultPrice,
                        Description = ss.Description,
                        Name = ss.Name,
                        WebsiteLink = ss.WebsiteLink
                    }).ToList(),
                Reviews = d.Reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    MediaTitle = r.MediaContent.Title,
                    WatchedOn = r.WatchHistory.WatchDate,
                    Comment = r.Comment,
                    Nickname = r.User.Nickname,
                }).ToList(),
                AudioOption = new OptionDTO
                {
                    Languages = d.AudioOption?.Languages
                },
                SubtitleOption = new OptionDTO
                {
                    Languages = d.SubtitleOption?.Languages
                }
            },
            Topics = d.Topics.Select(t => t.ToString()).ToList()
        }).ToList();
    }
    /* Get one media content by id including all details, with given id */
    public async Task<MediaContentDetailedDTO> GetMediaContentWithGivenIdAsync(int mediaId)
    {
        if (mediaId <= 0) throw new ArgumentException("Media id can not be equal or smaller than 0.");

        var mediaContent = await _mediaContentRepository.GetMediaContentWithGivenIdAsync(mediaId);
        if (mediaContent == null) throw new MediaContentDoesNotExistsException(new[] { mediaId });

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
        if (movie == null) throw new MovieDoesNotExistsException(movieId);

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

    private void ValidateTopics(ICollection<string> topics)
    {
        if (topics == null || !topics.Any())
            throw new AtLeastOneTopicMustExistsException();
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

    private static HashSet<Topic> ParseTopics(IEnumerable<string> topics)
    {
        return topics
            .Select(t => Enum.TryParse<Topic>(t, true, out var result)
                ? result
                : throw new InvalidTopicException(t))
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

        if (country.Length != 2)
            throw new ArgumentException("Country must be an ISO alpha-2 code (e.g. \"US\", \"PL\").",
                nameof(country));

        if (duration <= 0)
            throw new ArgumentException("Duration must be positive.", nameof(duration));

        if (releaseDate == default)
            throw new ArgumentException("Valid release date required.", nameof(releaseDate));

        if (audioLanguages != null && !audioLanguages.Any())
            throw new ArgumentException("At least one audio language is required.", nameof(audioLanguages));

        if (subtitleLanguages != null && !subtitleLanguages.Any())
            throw new ArgumentException("At least one subtitle language is required.", nameof(subtitleLanguages));
    }

    private void ValidateChanges(UpdateMovieDTO movieDto, Movie movie)
    {
        var newGenres = ParseGenres(movieDto.Genres);
        bool genresEqual = new HashSet<Genre>(newGenres)
            .SetEquals(movie.Genres);

        var mc = movieDto.MediaContent;
        bool titleEqual = string.Equals(mc.Title, movie.Title, StringComparison.OrdinalIgnoreCase);
        bool descEqual = string.Equals(mc.Description, movie.Description, StringComparison.OrdinalIgnoreCase);
        bool releaseDateEqual = mc.ReleaseDate == movie.ReleaseDate;
        bool langEqual = string.Equals(mc.OriginalLanguage, movie.OriginalLanguage, StringComparison.OrdinalIgnoreCase);
        bool countryEqual = string.Equals(mc.Country, movie.Country, StringComparison.OrdinalIgnoreCase);
        bool durationEqual = mc.Duration == movie.Duration;

        bool audioEqual;
        if (mc.AudioOption?.Languages == null && movie.AudioOption?.Languages == null)
            audioEqual = true;
        else if (mc.AudioOption?.Languages == null || movie.AudioOption?.Languages == null)
            audioEqual = false;
        else
            audioEqual = new HashSet<string>(mc.AudioOption.Languages, StringComparer.OrdinalIgnoreCase)
                .SetEquals(movie.AudioOption.Languages);

        bool subtitleEqual;
        if (mc.SubtitleOption?.Languages == null && movie.SubtitleOption?.Languages == null)
            subtitleEqual = true;
        else if (mc.SubtitleOption?.Languages == null || movie.SubtitleOption?.Languages == null)
            subtitleEqual = false;
        else
            subtitleEqual = new HashSet<string>(mc.SubtitleOption.Languages, StringComparer.OrdinalIgnoreCase)
                .SetEquals(movie.SubtitleOption.Languages);

        var currentIds = new HashSet<int>(movie.StreamingServices.Select(s => s.Id));
        var newIds = new HashSet<int>(mc.StreamingServiceIds);
        bool servicesEqual = currentIds.SetEquals(newIds);

        bool posterEqual = string.Equals(mc.PosterImageName, movie.PosterImageName, StringComparison.OrdinalIgnoreCase);
        bool trailerEqual = string.Equals(mc.YoutubeTrailerURL, movie.YoutubeTrailerURL,
            StringComparison.OrdinalIgnoreCase);

        if (genresEqual
            && titleEqual
            && descEqual
            && releaseDateEqual
            && langEqual
            && countryEqual
            && durationEqual
            && audioEqual
            && subtitleEqual
            && servicesEqual
            && posterEqual
            && trailerEqual)
        {
            throw new NoChangesDetectedException();
        }
    }

    private void ValidateDocumentaryChanges(UpdateDocumentaryDTO dto, Documentary documentary)
    {
        var newTopics = ParseTopics(dto.Topics);
        bool topicsEqual = new HashSet<Topic>(newTopics)
            .SetEquals(documentary.Topics);

        var mc = dto.MediaContent;
        bool titleEqual = string.Equals(mc.Title, documentary.Title, StringComparison.OrdinalIgnoreCase);
        bool descEqual = string.Equals(mc.Description, documentary.Description, StringComparison.OrdinalIgnoreCase);
        bool releaseDateEqual = mc.ReleaseDate == documentary.ReleaseDate;
        bool langEqual = string.Equals(mc.OriginalLanguage, documentary.OriginalLanguage,
            StringComparison.OrdinalIgnoreCase);
        bool countryEqual = string.Equals(mc.Country, documentary.Country, StringComparison.OrdinalIgnoreCase);
        bool durationEqual = mc.Duration == documentary.Duration;
        bool posterEqual = string.Equals(mc.PosterImageName, documentary.PosterImageName,
            StringComparison.OrdinalIgnoreCase);
        bool trailerEqual = string.Equals(mc.YoutubeTrailerURL, documentary.YoutubeTrailerURL,
            StringComparison.OrdinalIgnoreCase);

        bool audioEqual;
        if (mc.AudioOption?.Languages == null && documentary.AudioOption?.Languages == null)
            audioEqual = true;
        else if (mc.AudioOption?.Languages == null || documentary.AudioOption?.Languages == null)
            audioEqual = false;
        else
            audioEqual = new HashSet<string>(mc.AudioOption.Languages, StringComparer.OrdinalIgnoreCase)
                .SetEquals(documentary.AudioOption.Languages);

        bool subtitleEqual;
        if (mc.SubtitleOption?.Languages == null && documentary.SubtitleOption?.Languages == null)
            subtitleEqual = true;
        else if (mc.SubtitleOption?.Languages == null || documentary.SubtitleOption?.Languages == null)
            subtitleEqual = false;
        else
            subtitleEqual = new HashSet<string>(mc.SubtitleOption.Languages, StringComparer.OrdinalIgnoreCase)
                .SetEquals(documentary.SubtitleOption.Languages);

        var existingServiceIds = new HashSet<int>(documentary.StreamingServices.Select(s => s.Id));
        var desiredServiceIds = new HashSet<int>(mc.StreamingServiceIds ?? Enumerable.Empty<int>());
        bool servicesEqual = existingServiceIds.SetEquals(desiredServiceIds);

        if (topicsEqual
            && titleEqual
            && descEqual
            && releaseDateEqual
            && langEqual
            && countryEqual
            && durationEqual
            && audioEqual
            && subtitleEqual
            && servicesEqual
            && posterEqual
            && trailerEqual)
        {
            throw new NoChangesDetectedException();
        }
    }
}