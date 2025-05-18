using Project.Helper;
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
                    Country = "TR", Status = Status.Student
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
                    Name = "Apple TV", Country = "US", Description = "Premium streaming by Apple", DefaultPrice = 5.99m,
                    LogoImage = "apple-tv-logo"
                },
                new()
                {
                    Name = "Disney Plus", Country = "US", Description = "Family and Marvel content", DefaultPrice = 24.99m,
                    LogoImage = "disney-plus-logo"
                },
                new()
                {
                    Name = "HBO Max", Country = "US", Description = "HBO Originals and more", LogoImage = "max-logo",DefaultPrice = 9.99m,
                },
            };
            context.StreamingServices.AddRange(services);
            context.SaveChanges();
        }

        if (!context.Subscriptions.Any())
        {
            var subs = new List<Subscription>
            {
                new() {  StreamingServiceId = context.StreamingServices.First().Id },
                new() {  StreamingServiceId = context.StreamingServices.Skip(1).First().Id },
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
                    EndTime = DateTime.UtcNow.AddDays(25),
                    Price = SubscriptionPriceCalculator.CalculateAmount(context.StreamingServices.First().DefaultPrice, demirhan)
                });

                confirmations.Add(new SubscriptionConfirmation
                {
                    UserId = demirhan.Id,
                    SubscriptionId = subscriptions[1].Id,
                    PaymentMethod = "PayPal",
                    StartTime = DateTime.UtcNow.AddMonths(-1),
                    EndTime = DateTime.UtcNow.AddMonths(2),
                    Price = SubscriptionPriceCalculator.CalculateAmount(context.StreamingServices.Skip(1).First().DefaultPrice, demirhan)

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
                    Description =
                        "After the sudden death of his beloved wife, retired assassin John Wick receives one last gift from her—a beagle puppy named Daisy. But when a group of ruthless mobsters break into his home, steal his car, and kill Daisy, they unknowingly reawaken one of the deadliest killers the underworld has ever known. What follows is a relentless, high-octane tale of revenge as John Wick hunts down everyone involved, igniting a war against a powerful Russian crime syndicate.",
                    ReleaseDate = new DateTime(2014, 10, 24),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 101,
                    YoutubeTrailerURL = "C0BMx-qxsP4",
                    PosterImageName = "john-wick-poster",
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
                    Description = "Wade Wilson, a former Special Forces operative turned mercenary, is subjected to a rogue experiment that leaves him disfigured but with accelerated healing powers. Adopting the alter ego Deadpool, Wade embarks on a mission to hunt down the man who nearly destroyed his life. Known for his irreverent humor, fourth-wall-breaking antics, and ultra-violent combat skills, Deadpool is unlike any superhero you’ve seen before—raw, unfiltered, and wildly entertaining.",
                    ReleaseDate = new DateTime(2016, 2, 12),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 108,
                    YoutubeTrailerURL = "VHAK-gU9gi0",
                    PosterImageName = "deadpool-poster",
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
                    Description = "After being bitten by a genetically modified spider, awkward and intelligent teenager Peter Parker gains spider-like abilities. Struggling to balance his newfound powers, personal life, and responsibility, he takes on the mantle of Spider-Man to protect New York City from rising threats. As he battles foes and faces heartbreaking loss, Peter learns that with great power comes great responsibility.",
                    ReleaseDate = new DateTime(2017, 2, 12),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 108,
                    YoutubeTrailerURL = "t06RUxPbp_c",
                    PosterImageName = "spider-man-poster",
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
                    Description = "Quentin Tarantino’s cult masterpiece weaves together several interrelated stories involving Los Angeles mobsters, fringe criminals, and a mysterious briefcase. From the philosophical hitmen Vincent and Jules, to the washed-up boxer Butch, and the volatile duo of Pumpkin and Honey Bunny, 'Pulp Fiction' is a darkly comedic, non-linear exploration of crime, redemption, and pop culture that redefined modern cinema.",
                    ReleaseDate = new DateTime(1994, 10, 14),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 154,
                    YoutubeTrailerURL = "s7EdQ4FqbhY",
                    PosterImageName = "pulpfiction-poster",
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
                    Description = "When an unexpected threat emerges to endanger global security, Nick Fury, director of S.H.I.E.L.D., assembles a team of extraordinary individuals: Iron Man, Captain America, Thor, Hulk, Black Widow, and Hawkeye. Together, they must overcome their personal differences and unite to stop Loki, the god of mischief, from enslaving Earth with an alien army. 'The Avengers' is a thrilling culmination of Marvel’s cinematic universe, delivering action, wit, and superhero spectacle.",
                    ReleaseDate = new DateTime(2012, 5, 4),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 143,
                    YoutubeTrailerURL = "eOrNdBpGMv8&t=2s",
                    PosterImageName = "avengers-poster",
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
                    Description = "Regarded as one of the greatest films in cinematic history, 'The Godfather' chronicles the powerful Corleone crime family in post-war America. When patriarch Vito Corleone survives an assassination attempt, his reluctant son Michael is drawn into the brutal world of organized crime. As Michael rises to power, he transforms from an outsider to a ruthless mafia boss, sacrificing his morals and loved ones in the name of family loyalty and legacy.",

                    ReleaseDate = new DateTime(1972, 3, 24),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 175,
                    YoutubeTrailerURL = "UaVTIH8mujA",
                    PosterImageName = "godfather-poster",
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
                    Description = "Is a gangster drama directed by Brian De Palma in 1983 - inspired by the famous 1932 Howard Hawks film - was not a box office success, but today it is considered one of the most important productions in the oeuvre of the author of \"Carrie\". The story of the rapid criminal career of a small-time thug and its even faster end has gained the status of a cult film for many viewers, the best proof of which is the fact that the special edition DVD with the film sold like hot cakes in the USA. The hero of \"Scarface\" is a Cuban, Antonio Montana, who in 1981, along with thousands of other emigrants, comes to the USA.",
                    ReleaseDate = new DateTime(1983, 12, 9),
                    OriginalLanguage = "English",
                    Country = "USA",
                    Duration = 170,
                    YoutubeTrailerURL = "cv276Wg3e7I",
                    PosterImageName = "scarface-poster",
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
                    Title = "Vaultive",
                    Description = "You were expecting plot, drama, maybe some explosions? Nope. Just smooth vocals and betrayal.",
                    ReleaseDate = new DateTime(2025, 5, 9),
                    OriginalLanguage = "English",
                    Country = "Turkish",
                    Duration = 125,
                    YoutubeTrailerURL = "dQw4w9WgXcQ",
                    PosterImageName = null,
                    Genres = new HashSet<Genre> { Genre.Action },
                    SubtitleOption = new SubtitleOption
                    {
                        MediaTitle = "Vaultive",
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
            var vaultive = context.Movies.FirstOrDefault(m => m.Title == "Vaultive");

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

        if (!context.WatchHistories.Any())
        {
            var demirhan = context.Users.FirstOrDefault(u => u.Email == "demirhan@example.com");
            var watchedMovies = context.Movies
                .Where(m => m.Title == "John Wick" || m.Title == "Avengers" || m.Title == "Pulp Fiction")
                .ToList();

            var histories = new List<WatchHistory>();

            if (demirhan != null)
            {
                foreach (var movie in watchedMovies)
                {
                    histories.Add(new WatchHistory
                    {
                        UserId = demirhan.Id,
                        MediaTitle = movie.Title,
                        WatchDate = DateTime.UtcNow.AddDays(-watchedMovies.IndexOf(movie) - 1),
                        TimeLeftOf = movie.Duration - 20 
                    });
                }
            }

            context.WatchHistories.AddRange(histories);
            context.SaveChanges();
        }

    }
}