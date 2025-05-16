using Project.Models.Enumerations;
using Project.Models;

namespace Project.Context;

public static class SampleData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MasterContext>();

        // Create only one user: Demirhan Yalcin
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new()
                {
                    Firstname = "Demirhan", Lastname = "Yalcin", Nickname = "Demir", Email = "demirhan@example.com",
                    Country = "TR", Status = Status.Normal
                },
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        if (!context.StreamingServices.Any())
        {
            var services = new List<StreamingService>
            {
                new()
                {
                    Name = "Apple TV", Country = "US", Description = "Premium streaming by Apple",
                    LogoImage = "apple-tv-logo"
                },
                new()
                {
                    Name = "Disney Plus", Country = "US", Description = "Family and Marvel content",
                    LogoImage = "disney-plus-logo"
                },
                new()
                {
                    Name = "HBO Max", Country = "US", Description = "HBO Originals and more", LogoImage = "max-logo"
                },
            };
            context.StreamingServices.AddRange(services);
            context.SaveChanges();
        }

        if (!context.Subscriptions.Any())
        {
            var subs = new List<Subscription>
            {
                new() { DefaultPrice = 9.99, StreamingServiceId = context.StreamingServices.First().Id },
                new() { DefaultPrice = 24.99, StreamingServiceId = context.StreamingServices.Skip(1).First().Id },
                new() { DefaultPrice = 5.99, StreamingServiceId = context.StreamingServices.First().Id },
            };
            context.Subscriptions.AddRange(subs);
            context.SaveChanges();
        }

        if (!context.SubscriptionConfirmations.Any())
        {
            var demirhan = context.Users.FirstOrDefault(u => u.Email == "demirhan@example.com");
            var subscriptions = context.Subscriptions.ToList();

            var confirmations = new List<SubscriptionConfirmation>();

            if (demirhan != null)
            {
                confirmations.Add(new SubscriptionConfirmation
                {
                    UserId = demirhan.Id,
                    SubscriptionId = subscriptions[0].Id,
                    PaymentMethod = "CreditCard",
                    StartTime = DateTime.UtcNow.AddDays(-5),
                    EndTime = DateTime.UtcNow.AddDays(25)
                });

                confirmations.Add(new SubscriptionConfirmation
                {
                    UserId = demirhan.Id,
                    SubscriptionId = subscriptions[1].Id,
                    PaymentMethod = "PayPal",
                    StartTime = DateTime.UtcNow.AddMonths(-1),
                    EndTime = DateTime.UtcNow.AddMonths(2)
                });
            }

            context.SubscriptionConfirmations.AddRange(confirmations);
            context.SaveChanges();
        }

        if (!context.Movies.Any())
        {
            var movies = new List<Movie>
            {
                new Movie
                {
                    Title = "John Wick",
                    Description = "John Wick is a former assassin drawn back into the criminal underworld...",
                    ReleaseDate = new DateTime(2014, 10, 24),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 101,
                    BackgroundImage = "john-wick-background",
                    PosterImage = "john-wick-poster",
                    Genres = new HashSet<Genre> { Genre.Action, Genre.Thriller },
                    SubtitleOption = new SubtitleOption
                    {
                        MediaTitle = "John Wick",
                        SubtitleLanguages = new List<SubtitleLanguage>
                        {
                            new() { Language = "English" }
                        }
                    }
                },
                new Movie
                {
                    Title = "Deadpool",
                    Description = "Deadpool tells the story of Wade Wilson, a former special forces operative...",
                    ReleaseDate = new DateTime(2016, 2, 12),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 108,
                    BackgroundImage = "deadpool-background",
                    PosterImage = "deadpool-poster",
                    Genres = new HashSet<Genre> { Genre.Action, Genre.Comedy, Genre.Superhero },
                    SubtitleOption = new SubtitleOption
                    {
                        MediaTitle = "Deadpool",
                        SubtitleLanguages = new List<SubtitleLanguage>
                        {
                            new() { Language = "English" }
                        }
                    }
                },
                new Movie
                {
                    Title = "Spiderman",
                    Description = "Hero wearing red cape.",
                    ReleaseDate = new DateTime(2017, 2, 12),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 108,
                    BackgroundImage = "spider-man-background",
                    PosterImage = "spider-man-poster",
                    Genres = new HashSet<Genre> { Genre.Action, Genre.Superhero },
                    SubtitleOption = new SubtitleOption
                    {
                        MediaTitle = "Spiderman",
                        SubtitleLanguages = new List<SubtitleLanguage>
                        {
                            new() { Language = "English" }
                        }
                    }
                },
                new Movie
                {
                    Title = "Pulp Fiction",
                    Description = "Pulp Fiction is a cult classic directed by Quentin Tarantino...",
                    ReleaseDate = new DateTime(1994, 10, 14),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 154,
                    BackgroundImage = "pulpfiction-background",
                    PosterImage = "pulpfiction-poster",
                    Genres = new HashSet<Genre> { Genre.Crime, Genre.Drama },
                    SubtitleOption = new SubtitleOption
                    {
                        MediaTitle = "Pulp Fiction",
                        SubtitleLanguages = new List<SubtitleLanguage>
                        {
                            new() { Language = "English" }
                        }
                    }
                },
                new Movie
                {
                    Title = "Avengers",
                    Description = "Avengers brings together Marvel's greatest superheroes in an epic battle...",
                    ReleaseDate = new DateTime(2012, 5, 4),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 143,
                    BackgroundImage = "avengers-background",
                    PosterImage = "avengers-poster",
                    Genres = new HashSet<Genre> { Genre.Action, Genre.Superhero, Genre.SciFi },
                    SubtitleOption = new SubtitleOption
                    {
                        MediaTitle = "Avengers",
                        SubtitleLanguages = new List<SubtitleLanguage>
                        {
                            new() { Language = "English" }
                        }
                    }
                },
                new Movie
                {
                    Title = "God Father",
                    Description =
                        "The Godfather is a cinematic masterpiece that chronicles the rise of Michael Corleone...",
                    ReleaseDate = new DateTime(1972, 3, 24),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 175,
                    BackgroundImage = "godfather-background",
                    PosterImage = "godfather-poster",
                    Genres = new HashSet<Genre> { Genre.Crime, Genre.Drama },
                    SubtitleOption = new SubtitleOption
                    {
                        MediaTitle = "God Father",
                        SubtitleLanguages = new List<SubtitleLanguage>
                        {
                            new() { Language = "English" }
                        }
                    }
                },
                new Movie
                {
                    Title = "Scarface",
                    Description = "Scarface follows the rise and fall of Tony Montana, a Cuban immigrant...",
                    ReleaseDate = new DateTime(1983, 12, 9),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 170,
                    BackgroundImage = "scarface-background",
                    PosterImage = "scarface-poster",
                    Genres = new HashSet<Genre> { Genre.Crime, Genre.Drama },
                    SubtitleOption = new SubtitleOption
                    {
                        MediaTitle = "Scarface",
                        SubtitleLanguages = new List<SubtitleLanguage>
                        {
                            new() { Language = "English" }
                        }
                    }
                },
                new Movie
                {
                    Title = "Vaultive Special",
                    Description = "We love this platform!",
                    ReleaseDate = new DateTime(2025, 5, 9),
                    OriginalLanguage = "English",
                    Country = "Turkish",
                    Duration = 125,
                    BackgroundImage = "",
                    PosterImage = "",
                    Genres = new HashSet<Genre> { Genre.Action },
                    SubtitleOption = new SubtitleOption
                    {
                        MediaTitle = "Vaultive Special",
                        SubtitleLanguages = new List<SubtitleLanguage>
                        {
                            new() { Language = "English" }
                        }
                    }
                }
            };
            context.Movies.AddRange(movies);
            context.SaveChanges();
        }

        if (!context.MediaContentStreamingServices.Any())
        {
            var appleTV = context.StreamingServices.FirstOrDefault(s => s.Name == "Apple TV");
            var disney = context.StreamingServices.FirstOrDefault(s => s.Name == "Disney Plus");
            var hbo = context.StreamingServices.FirstOrDefault(s => s.Name == "HBO Max");

            var johnWick = context.Movies.FirstOrDefault(m => m.Title == "John Wick");
            var deadpool = context.Movies.FirstOrDefault(m => m.Title == "Deadpool");
            var avengers = context.Movies.FirstOrDefault(m => m.Title == "Avengers");
            var godfather = context.Movies.FirstOrDefault(m => m.Title == "God Father");
            var pulpFiction = context.Movies.FirstOrDefault(m => m.Title == "Pulp Fiction");
            var scarface = context.Movies.FirstOrDefault(m => m.Title == "Scarface");
            var spiderman = context.Movies.FirstOrDefault(m => m.Title == "Spiderman");
            var vaultive = context.Movies.FirstOrDefault(m => m.Title == "Vaultive Special");

            var mcStreaming = new List<MediaContentStreamingService>();

            if (johnWick != null && appleTV != null && disney != null)
            {
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = johnWick.Title, StreamingServiceId = appleTV.Id });
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = johnWick.Title, StreamingServiceId = disney.Id });
            }

            if (deadpool != null && disney != null)
            {
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = deadpool.Title, StreamingServiceId = disney.Id });
            }

            if (avengers != null && disney != null && hbo != null)
            {
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = avengers.Title, StreamingServiceId = disney.Id });
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = avengers.Title, StreamingServiceId = hbo.Id });
            }

            if (godfather != null && hbo != null)
            {
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = godfather.Title, StreamingServiceId = hbo.Id });
            }

            if (pulpFiction != null && appleTV != null)
            {
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = pulpFiction.Title, StreamingServiceId = appleTV.Id });
            }

            if (scarface != null && hbo != null && appleTV != null)
            {
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = scarface.Title, StreamingServiceId = hbo.Id });
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = scarface.Title, StreamingServiceId = appleTV.Id });
            }

            if (spiderman != null && disney != null)
            {
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = spiderman.Title, StreamingServiceId = disney.Id });
            }

            if (vaultive != null && disney != null && hbo != null && appleTV != null)
            {
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = vaultive.Title, StreamingServiceId = disney.Id });
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = vaultive.Title, StreamingServiceId = hbo.Id });
                mcStreaming.Add(new MediaContentStreamingService
                    { MediaTitle = vaultive.Title, StreamingServiceId = appleTV.Id });
            }
            
            context.MediaContentStreamingServices.AddRange(mcStreaming);
            context.SaveChanges();
        }
    }
}