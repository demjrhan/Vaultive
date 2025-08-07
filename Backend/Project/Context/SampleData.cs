using Microsoft.EntityFrameworkCore;
using Project.Models.Enumerations;
using Project.Models;
using Project.Services;

namespace Project.Context;

public static class SampleData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MasterContext>();

        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new()
                {
                    Firstname = "Demirhan",
                    Lastname = "Yalcin",
                    Nickname = "Demir",
                    Email = "demirhan@example.com",
                    Country = "TR",
                    Status = Status.Student
                },
                new()
                {
                    Firstname = "Aiko",
                    Lastname = "Tanaka",
                    Nickname = "AikoT",
                    Email = "aiko.tanaka@example.jp",
                    Country = "JP",
                    Status = Status.Student
                },
                new()
                {
                    Firstname = "Michał",
                    Lastname = "Nowak",
                    Nickname = "MikeN",
                    Email = "michal.nowak@example.pl",
                    Country = "PL",
                    Status = Status.Elder
                }
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
                    LogoImage = "apple-tv-logo", WebsiteLink = "https://tv.apple.com/pl"
                },
                new()
                {
                    Name = "Disney Plus", Country = "US", Description = "Family and Marvel content",
                    DefaultPrice = 24.99m,
                    LogoImage = "disney-plus-logo", WebsiteLink = "https://www.disneyplus.com/en-de"
                },
                new()
                {
                    Name = "HBO Max", Country = "US", Description = "HBO Originals and more", LogoImage = "max-logo",
                    DefaultPrice = 9.99m, WebsiteLink = "https://www.max.com/pl/pl"
                },
            };
            context.StreamingServices.AddRange(services);
            context.SaveChanges();
        }

        var demir = context.Users.FirstOrDefault(u => u.Email == "demirhan@example.com");
        var aiko = context.Users.FirstOrDefault(u => u.Email == "aiko.tanaka@example.jp");
        var michal = context.Users.FirstOrDefault(u => u.Email == "michal.nowak@example.pl");

        var appleTV = context.StreamingServices.FirstOrDefault(s => s.Name == "Apple TV");
        var disney = context.StreamingServices.FirstOrDefault(s => s.Name == "Disney Plus");
        var hbo = context.StreamingServices.FirstOrDefault(s => s.Name == "HBO Max");

        if (!context.Subscriptions.Any())
        {
            if (demir != null && aiko != null && michal != null &&
                appleTV != null && disney != null && hbo != null)
            {
                var subscriptions = new List<Subscription>
                {
                    new() { StreamingServiceId = appleTV.Id },
                    new() { StreamingServiceId = disney.Id },
                    new() { StreamingServiceId = hbo.Id },
                    new() { StreamingServiceId = appleTV.Id }
                };

                context.Subscriptions.AddRange(subscriptions);
                context.SaveChanges();

                context.SubscriptionConfirmations.AddRange(new List<SubscriptionConfirmation>
                {
                    new()
                    {
                        UserId = demir.Id,
                        SubscriptionId = subscriptions[0].Id,
                        PaymentMethod = "CreditCard",
                        StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-155)),
                        EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-130)),
                        Price = SubscriptionService.CalculateAmountOfConfirmation(appleTV.DefaultPrice, demir)
                    },
                    new()
                    {
                        UserId = demir.Id,
                        SubscriptionId = subscriptions[0].Id,
                        PaymentMethod = "CreditCard",
                        StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                        EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20)),
                        Price = SubscriptionService.CalculateAmountOfConfirmation(appleTV.DefaultPrice, demir)
                    },
                    new()
                    {
                        UserId = aiko.Id,
                        SubscriptionId = subscriptions[1].Id,
                        PaymentMethod = "PayPal",
                        StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                        EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(25)),
                        Price = SubscriptionService.CalculateAmountOfConfirmation(disney.DefaultPrice, aiko)
                    },
                    new()
                    {
                        UserId = aiko.Id,
                        SubscriptionId = subscriptions[3].Id,
                        PaymentMethod = "PayPal",
                        StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                        EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(25)),
                        Price = SubscriptionService.CalculateAmountOfConfirmation(disney.DefaultPrice, aiko)
                    },
                    new()
                    {
                        UserId = michal.Id,
                        SubscriptionId = subscriptions[2].Id,
                        PaymentMethod = "DebitCard",
                        StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-45)),
                        EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15)),
                        Price = SubscriptionService.CalculateAmountOfConfirmation(hbo.DefaultPrice, michal)
                    }
                });

                context.SaveChanges();
            }
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
                    ReleaseDate = new DateOnly(2014, 10, 24),
                    OriginalLanguage = "English",
                    Country = "US",
                    Duration = 101,
                    YoutubeTrailerURL = "C0BMx-qxsP4",
                    PosterImageName = "john-wick-poster",
                    Genres = new List<Genre> { Genre.Action, Genre.Thriller },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>() { "English" }
                    }
                },
                new Movie
                {
                    Title = "Deadpool",
                    Description =
                        "Wade Wilson, a former Special Forces operative turned mercenary, is subjected to a rogue experiment that leaves him disfigured but with accelerated healing powers. Adopting the alter ego Deadpool, Wade embarks on a mission to hunt down the man who nearly destroyed his life. Known for his irreverent humor, fourth-wall-breaking antics, and ultra-violent combat skills, Deadpool is unlike any superhero you’ve seen before—raw, unfiltered, and wildly entertaining.",
                    ReleaseDate = new DateOnly(2016, 2, 12),
                    OriginalLanguage = "English",
                    Country = "US",
                    Duration = 108,
                    YoutubeTrailerURL = "VHAK-gU9gi0",
                    PosterImageName = "deadpool-poster",
                    Genres = new List<Genre> { Genre.Action, Genre.Comedy, Genre.Superhero },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>() { "English" }
                    }
                },
                new Movie
                {
                    Title = "Spiderman",
                    Description =
                        "After being bitten by a genetically modified spider, awkward and intelligent teenager Peter Parker gains spider-like abilities. Struggling to balance his newfound powers, personal life, and responsibility, he takes on the mantle of Spider-Man to protect New York City from rising threats. As he battles foes and faces heartbreaking loss, Peter learns that with great power comes great responsibility.",
                    ReleaseDate = new DateOnly(2017, 2, 12),
                    OriginalLanguage = "English",
                    Country = "US",
                    Duration = 108,
                    YoutubeTrailerURL = "t06RUxPbp_c",
                    PosterImageName = "spider-man-poster",
                    Genres = new List<Genre> { Genre.Action, Genre.Superhero },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>() { "English" }
                    }
                },
                new Movie
                {
                    Title = "Pulp Fiction",
                    Description =
                        "Quentin Tarantino’s cult masterpiece weaves together several interrelated stories involving Los Angeles mobsters, fringe criminals, and a mysterious briefcase. From the philosophical hitmen Vincent and Jules, to the washed-up boxer Butch, and the volatile duo of Pumpkin and Honey Bunny, 'Pulp Fiction' is a darkly comedic, non-linear exploration of crime, redemption, and pop culture that redefined modern cinema.",
                    ReleaseDate = new DateOnly(1994, 10, 14),
                    OriginalLanguage = "English",
                    Country = "US",
                    Duration = 154,
                    YoutubeTrailerURL = "s7EdQ4FqbhY",
                    PosterImageName = "pulpfiction-poster",
                    Genres = new List<Genre> { Genre.Crime, Genre.Drama },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>() { "English" }
                    }
                },
                new Movie
                {
                    Title = "Avengers",
                    Description =
                        "When an unexpected threat emerges to endanger global security, Nick Fury, director of S.H.I.E.L.D., assembles a team of extraordinary individuals: Iron Man, Captain America, Thor, Hulk, Black Widow, and Hawkeye. Together, they must overcome their personal differences and unite to stop Loki, the god of mischief, from enslaving Earth with an alien army. 'The Avengers' is a thrilling culmination of Marvel’s cinematic universe, delivering action, wit, and superhero spectacle.",
                    ReleaseDate = new DateOnly(2012, 5, 4),
                    OriginalLanguage = "English",
                    Country = "US",
                    Duration = 143,
                    YoutubeTrailerURL = "6ZfuNTqbHE8",
                    PosterImageName = "avengers-poster",
                    Genres = new List<Genre> { Genre.Action, Genre.Superhero, Genre.SciFi },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>() { "English" }
                    }
                },
                new Movie
                {
                    Title = "God Father",
                    Description =
                        "Regarded as one of the greatest films in cinematic history, 'The Godfather' chronicles the powerful Corleone crime family in post-war America. When patriarch Vito Corleone survives an assassination attempt, his reluctant son Michael is drawn into the brutal world of organized crime. As Michael rises to power, he transforms from an outsider to a ruthless mafia boss, sacrificing his morals and loved ones in the name of family loyalty and legacy.",

                    ReleaseDate = new DateOnly(1972, 3, 24),
                    OriginalLanguage = "English",
                    Country = "US",
                    Duration = 175,
                    YoutubeTrailerURL = "UaVTIH8mujA",
                    PosterImageName = "godfather-poster",
                    Genres = new List<Genre> { Genre.Crime, Genre.Drama },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>() { "English" }
                    }
                },
                new Movie
                {
                    Title = "Scarface",
                    Description =
                        "Is a gangster drama directed by Brian De Palma in 1983 - inspired by the famous 1932 Howard Hawks film - was not a box office success, but today it is considered one of the most important productions in the oeuvre of the author of \"Carrie\". The story of the rapid criminal career of a small-time thug and its even faster end has gained the status of a cult film for many viewers, the best proof of which is the fact that the special edition DVD with the film sold like hot cakes in the USA. The hero of \"Scarface\" is a Cuban, Antonio Montana, who in 1981, along with thousands of other emigrants, comes to the USA.",
                    ReleaseDate = new DateOnly(1983, 12, 9),
                    OriginalLanguage = "English",
                    Country = "US",
                    Duration = 170,
                    YoutubeTrailerURL = "cv276Wg3e7I",
                    PosterImageName = "scarface-poster",
                    Genres = new List<Genre> { Genre.Crime, Genre.Drama },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>() { "English", "Spanish" }
                    },
                    AudioOption = new AudioOption()
                    {
                        Languages = new List<string>() { "Japanese" }
                    }
                },
                new Movie
                {
                    Title = "The Wolf of Wall Street",
                    Description =
                        "Based on the true story of Jordan Belfort, this film chronicles his rise and fall as a wealthy stockbroker living the high life before his dramatic collapse.",
                    ReleaseDate = new DateOnly(2013, 12, 25),
                    OriginalLanguage = "English",
                    Country = "United States",
                    Duration = 180,
                    YoutubeTrailerURL = "iszwuX1AK6A",
                    PosterImageName = "wolf-of-wall-street",
                    Genres = new List<Genre>
                    {
                        Genre.Biography,
                        Genre.Comedy,
                        Genre.Drama
                    },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "Spanish",
                            "French",
                            "German"
                        }
                    }
                },
                new Movie
                {
                    Title = "Training Day",
                    Description =
                        "A rookie LAPD narcotics officer spends his first day with a rogue detective who isn’t what he appears to be, testing his morals and resolve.",
                    ReleaseDate = new DateOnly(2001, 10, 5),
                    OriginalLanguage = "English",
                    Country = "United States",
                    Duration = 122,
                    YoutubeTrailerURL = "DXPJqRtkDP0",
                    PosterImageName = "training-day-poster",
                    Genres = new List<Genre>
                    {
                        Genre.Crime,
                        Genre.Drama,
                        Genre.Thriller
                    },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "Spanish",
                            "French"
                        }
                    }
                },
                new Movie
                {
                    Title = "Despereaux",
                    Description =
                        "A brave mouse helps to restore happiness to a forlorn kingdom by befriending a princess and a gentleman rat.",
                    ReleaseDate = new DateOnly(2008, 12, 19),
                    OriginalLanguage = "English",
                    Country = "United Kingdom, United States",
                    Duration = 93,
                    YoutubeTrailerURL = "I7b-Vfz0ga4",
                    PosterImageName = "despereaux-poster",
                    Genres = new List<Genre>
                    {
                        Genre.Animation,
                        Genre.Family,
                        Genre.Fantasy
                    },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "Spanish"
                        }
                    },
                    AudioOption = new AudioOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "French"
                        }
                    }
                },
                new Movie
                {
                    Title = "Moana 2",
                    Description =
                        "Set three years after the first film, Moana reunites with the demigod Maui and assembles a wayfinding crew to find the lost island of Motufetu, break its curse, and reconnect the people of the ocean.",
                    ReleaseDate = new DateOnly(2024, 11, 27),
                    OriginalLanguage = "English",
                    Country = "United States",
                    Duration = 100,
                    YoutubeTrailerURL = "hDZ7y8RP5HE",
                    PosterImageName = "moana-2-poster",
                    Genres = new List<Genre>
                    {
                        Genre.Action,
                        Genre.Animation,
                        Genre.Family,
                        Genre.Fantasy
                    },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "Spanish",
                            "French",
                            "German"
                        }
                    }
                },
                new Movie
                {
                    Title = "Cars",
                    Description =
                        "A hotshot race-car named Lightning McQueen becomes stranded in the small town of Radiator Springs, where he learns life's true priorities.",
                    ReleaseDate = new DateOnly(2006, 6, 9),
                    OriginalLanguage = "English",
                    Country = "United States",
                    Duration = 117,
                    YoutubeTrailerURL = "W_H7_tDHFE8",
                    PosterImageName = "cars-poster",
                    Genres = new List<Genre>
                    {
                        Genre.Animation,
                        Genre.Adventure,
                        Genre.Comedy,
                        Genre.Family
                    },
                    State = State.Published,
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "Spanish",
                            "French"
                        }
                    },
                    AudioOption = new AudioOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "Spanish"
                        }
                    }
                },
                new Movie  
                {  
                    Title = "Ratatouille",  
                    Description = "A rat named Remy dreams of becoming a great French chef despite his family's wishes and the obvious problem of being a rat in a decidedly rodent-phobic profession.",  
                    ReleaseDate = new DateOnly(2007, 6, 29),  
                    OriginalLanguage = "English",  
                    Country = "United States",  
                    Duration = 111,  
                    YoutubeTrailerURL = "NgsQ8mVkN8w",  
                    PosterImageName = "ratatouille-poster",  
                    State = State.Published,
                    Genres = new List<Genre>  
                    {  
                        Genre.Animation,  
                        Genre.Comedy,  
                        Genre.Family  
                    },  
                    SubtitleOption = new SubtitleOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "French",  
                            "Spanish"  
                        }  
                    },  
                    AudioOption = new AudioOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "Spanish"  
                        }  
                    }  
                },
                new Movie  
                {  
                    Title = "Lilo & Stitch",  
                    Description = "A young Hawaiian girl adopts an unusual pet who is actually a notorious extraterrestrial fugitive. Through her love, faith, and unwavering belief in 'ohana' (family), she helps unlock his heart and gives him a chance at redemption.",  
                    ReleaseDate = new DateOnly(2002, 6, 21),  
                    OriginalLanguage = "English",  
                    Country = "United States",  
                    Duration = 85,  
                    YoutubeTrailerURL = "9OAC55UWAQs",  
                    PosterImageName = "lilo-and-stitch-poster",  
                    State = State.Published,
                    Genres = new List<Genre>  
                    {  
                        Genre.Animation,  
                        Genre.Adventure,  
                        Genre.Comedy,  
                        Genre.Family,  
                        Genre.SciFi  
                    },  
                    SubtitleOption = new SubtitleOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "Spanish",  
                            "French"  
                        }  
                    },  
                    AudioOption = new AudioOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "Spanish"  
                        }  
                    }  
                },
                new Movie  
                {  
                    Title = "Shrek",  
                    Description = "In a faraway swamp, an ogre named Shrek finds his peaceful life disrupted when a multitude of fairy tale creatures are exiled there by the evil Lord Farquaad. To reclaim his land, Shrek makes a deal to rescue Princess Fiona with the help of a talkative donkey — only to discover unexpected friendship, secrets, and love.",  
                    ReleaseDate = new DateOnly(2001, 5, 18),  
                    OriginalLanguage = "English",  
                    Country = "United States",  
                    Duration = 90,  
                    YoutubeTrailerURL = "CwXOrWvPBPk",  
                    PosterImageName = "shrek-poster", 
                    State = State.Published,
                    Genres = new List<Genre>  
                    {  
                        Genre.Animation,  
                        Genre.Comedy,  
                        Genre.Adventure,  
                        Genre.Fantasy,  
                        Genre.Family  
                    },  
                    SubtitleOption = new SubtitleOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "Spanish",  
                            "German"  
                        }  
                    },  
                    AudioOption = new AudioOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "Spanish"  
                        }  
                    }  
                },
                new Movie  
                {  
                    Title = "Ice Age",  
                    Description = "Set during the Ice Age, a woolly mammoth, a sloth, and a saber-toothed tiger form an unlikely herd as they embark on a journey to return a lost human baby to its tribe, encountering comic misadventures and building unexpected bonds along the way.",  
                    ReleaseDate = new DateOnly(2002, 3, 15),  
                    OriginalLanguage = "English",  
                    Country = "United States",  
                    Duration = 81,  
                    YoutubeTrailerURL = "i4noiCRJRoE",  
                    PosterImageName = "ice-age-poster",  
                    State = State.Published,
                    Genres = new List<Genre>  
                    {  
                        Genre.Animation,  
                        Genre.Adventure,  
                        Genre.Comedy,  
                        Genre.Family  
                    },  
                    SubtitleOption = new SubtitleOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "Spanish",  
                            "French"  
                        }  
                    },  
                    AudioOption = new AudioOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "Spanish"  
                        }  
                    }  
                },
                new Movie  
                {  
                    Title = "Up",  
                    Description = "An elderly widower named Carl sets out to fulfill his lifelong dream of visiting South America by tying thousands of balloons to his house. Accidentally accompanied by an earnest young Wilderness Explorer named Russell, Carl embarks on a heartwarming and unexpected adventure.",  
                    ReleaseDate = new DateOnly(2009, 5, 29),  
                    OriginalLanguage = "English",  
                    Country = "United States",  
                    Duration = 96,  
                    YoutubeTrailerURL = "HWEW_qTLSEE",  
                    PosterImageName = "up-poster",  
                    State = State.Published,
                    Genres = new List<Genre>  
                    {  
                        Genre.Animation,  
                        Genre.Adventure,  
                        Genre.Comedy,  
                        Genre.Family  
                    },  
                    SubtitleOption = new SubtitleOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "Spanish",  
                            "German"  
                        }  
                    },  
                    AudioOption = new AudioOption  
                    {  
                        Languages = new List<string>  
                        {  
                            "English",  
                            "Spanish"  
                        }  
                    }  
                },
                new Movie  
                {  
                    Title = "Kung Fu Panda",  
                    Description = "Po, a clumsy and food-loving panda, is unexpectedly chosen to fulfill an ancient prophecy. Trained by Master Shifu, he must embrace his destiny to become the Dragon Warrior and protect the Valley of Peace from the villainous Tai Lung.",  
                    ReleaseDate = new DateOnly(2008, 6, 6),  
                    OriginalLanguage = "English",  
                    Country = "United States",  
                    Duration = 92,  
                    YoutubeTrailerURL = "NRc-ze7Wrxw",  
                    PosterImageName = "kung-fu-panda-1-poster",
                    State = State.Published,
                    Genres = new List<Genre>  
                    {  
                        Genre.Animation,  
                        Genre.Action,  
                        Genre.Comedy,  
                        Genre.Family  
                    },  
                    SubtitleOption = new SubtitleOption  
                    {  
                        Languages = new List<string> { "English", "Spanish", "German" }  
                    },  
                    AudioOption = new AudioOption  
                    {  
                        Languages = new List<string> { "English", "Spanish" }  
                    }  
                },
                new Movie  
                {  
                    Title = "Kung Fu Panda 2",  
                    Description = "Po and the Furious Five set out on a mission to stop a villainous peacock named Lord Shen, who plans to use a powerful weapon to conquer China and destroy kung fu. Along the way, Po uncovers the truth about his past.",  
                    ReleaseDate = new DateOnly(2011, 5, 26),  
                    OriginalLanguage = "English",  
                    Country = "United States",  
                    Duration = 91,  
                    YoutubeTrailerURL = "FQ63rqSRrEI",  
                    PosterImageName = "kung-fu-panda-2-poster",  
                    State = State.Published,
                    Genres = new List<Genre>  
                    {  
                        Genre.Animation,  
                        Genre.Action,  
                        Genre.Comedy,  
                        Genre.Family  
                    },  
                    SubtitleOption = new SubtitleOption  
                    {  
                        Languages = new List<string> { "English", "French", "Spanish" }  
                    },  
                    AudioOption = new AudioOption  
                    {  
                        Languages = new List<string> { "English", "Spanish", "French" }  
                    }  
                }

            };
            var documentaries = new List<Documentary>
            {
                new Documentary
                {
                    Title = "The Social Dilemma",
                    Description =
                        "Explores the dangerous human impact of social networking, with tech experts sounding the alarm on their own creations.",
                    ReleaseDate = new DateOnly(2020, 9, 9),
                    OriginalLanguage = "English",
                    Country = "United States",
                    Duration = 94,
                    YoutubeTrailerURL = null,
                    PosterImageName = null,
                    State = State.Published,
                    Topics = new List<Topic>
                    {
                        Topic.Technology,
                        Topic.Society
                    },
                    AudioOption = new AudioOption
                    {
                        Languages = new List<string>
                        {
                            "English"
                        }
                    },
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "German"
                        }
                    }
                },
                new Documentary
                {
                    Title = "My Octopus Teacher",
                    Description =
                        "A filmmaker forges an unusual friendship with an octopus living in a South African kelp forest, learning about its world.",
                    ReleaseDate = new DateOnly(2020, 9, 7),
                    OriginalLanguage = "English",
                    Country = "South Africa",
                    Duration = 85,
                    YoutubeTrailerURL = null,
                    PosterImageName = null,
                    State = State.Published,
                    Topics = new List<Topic>
                    {
                        Topic.Nature,
                        Topic.Biology,
                    },
                    AudioOption = new AudioOption
                    {
                        Languages = new List<string>
                        {
                            "English"
                        }
                    },
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "Spanish"
                        }
                    }
                },
                new Documentary
                {
                    Title = "Seaspiracy",
                    Description =
                        "A filmmaker uncovers alarming global corruption and environmental destruction in the commercial fishing industry.",
                    ReleaseDate = new DateOnly(2021, 3, 24),
                    OriginalLanguage = "English",
                    Country = "United Kingdom",
                    Duration = 89,
                    YoutubeTrailerURL = null,
                    PosterImageName = null,
                    State = State.Archived,
                    Topics = new List<Topic>
                    {
                        Topic.Environment,
                        Topic.Economy,
                        Topic.Politics
                    },
                    AudioOption = new AudioOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "French"
                        }
                    },
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string>
                        {
                            "English",
                            "Italian",
                            "German"
                        }
                    }
                }
            };
            var shortFilms = new List<ShortFilm>
            {
                new ShortFilm
                {
                    Title = "Digital Pulse",
                    Description =
                        "An experimental look into the constant stream of information shaping modern youth in a digitally saturated society.",
                    ReleaseDate = new DateOnly(2022, 3, 14),
                    OriginalLanguage = "English",
                    Country = "Poland",
                    Duration = 12,
                    YoutubeTrailerURL = null,
                    PosterImageName = null,
                    SchoolName = "Polish Japanese Academy of Technology",
                    State = State.Published,
                    Genres = new List<Genre>
                    {
                        Genre.Drama,
                        Genre.Experiment,
                        Genre.SciFi
                    },
                    AudioOption = new AudioOption
                    {
                        Languages = new List<string> { "English", "Polish" }
                    },
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string> { "English", "Polish" }
                    }
                },
                new ShortFilm
                {
                    Title = "The Last Sketch",
                    Description =
                        "A forgotten artist’s final days in a rapidly changing urban landscape. Captured in stark black-and-white visuals.",
                    ReleaseDate = new DateOnly(2021, 11, 2),
                    OriginalLanguage = "Polish",
                    Country = "Poland",
                    Duration = 9,
                    YoutubeTrailerURL = null,
                    PosterImageName = null,
                    SchoolName = "Polish Japanese Academy of Technology",
                    State = State.Published,
                    Genres = new List<Genre>
                    {
                        Genre.Drama,
                        Genre.Biography
                    },
                    AudioOption = new AudioOption
                    {
                        Languages = new List<string> { "Polish" }
                    },
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string> { "English" }
                    }
                },
                new ShortFilm
                {
                    Title = "Echoes of Code",
                    Description =
                        "A poetic short following a coder’s internal monologue as they confront self-doubt and imposter syndrome during finals week.",
                    ReleaseDate = new DateOnly(2023, 6, 10),
                    OriginalLanguage = "English",
                    Country = "Poland",
                    Duration = 10,
                    YoutubeTrailerURL = null,
                    PosterImageName = null,
                    SchoolName = "Polish Japanese Academy of Technology",
                    State = State.Published,
                    Genres = new List<Genre>
                    {
                        Genre.Psychological,
                        Genre.Student,
                        Genre.Fantasy
                    },
                    AudioOption = new AudioOption
                    {
                        Languages = new List<string> { "English" }
                    },
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string> { "English", "Polish" }
                    }
                },
                new ShortFilm
                {
                    Title = "CTRL+Z",
                    Description =
                        "After a software glitch gives a student the power to rewind time, they must choose between fixing mistakes or accepting growth.",
                    ReleaseDate = new DateOnly(2022, 12, 20),
                    OriginalLanguage = "Polish",
                    Country = "Poland",
                    Duration = 15,
                    YoutubeTrailerURL = null,
                    PosterImageName = null,
                    SchoolName = "Polish Japanese Academy of Technology",
                    State = State.Published,
                    Genres = new List<Genre>
                    {
                        Genre.SciFi,
                        Genre.Drama,
                        Genre.Mystery
                    },
                    AudioOption = new AudioOption
                    {
                        Languages = new List<string> { "Polish", "English" }
                    },
                    SubtitleOption = new SubtitleOption
                    {
                        Languages = new List<string> { "English" }
                    }
                }
            };

            context.Movies.AddRange(movies);
            context.Documentaries.AddRange(documentaries);
            context.ShortFilms.AddRange(shortFilms);
            context.SaveChanges();
        }

        if (!context.Movies.Any(m => m.StreamingServices.Any()))
        {
            var johnWick = context.Movies.FirstOrDefault(m => m.Title == "John Wick");
            var deadpool = context.Movies.FirstOrDefault(m => m.Title == "Deadpool");
            var avengers = context.Movies.FirstOrDefault(m => m.Title == "Avengers");
            var godfather = context.Movies.FirstOrDefault(m => m.Title == "God Father");
            var pulpFiction = context.Movies.FirstOrDefault(m => m.Title == "Pulp Fiction");
            var scarface = context.Movies.FirstOrDefault(m => m.Title == "Scarface");
            var spiderman = context.Movies.FirstOrDefault(m => m.Title == "Spiderman");
            var wolfOfWallStreet = context.Movies.FirstOrDefault(m => m.Title == "The Wolf of Wall Street");
            var trainingDay = context.Movies.FirstOrDefault(m => m.Title == "Training Day");
            var despereaux = context.Movies.FirstOrDefault(m => m.Title == "Despereaux");
            var moana2 = context.Movies.FirstOrDefault(m => m.Title == "Moana 2");
            var cars = context.Movies.FirstOrDefault(m => m.Title == "Cars");
            var kungFuPanda1 = context.Movies.FirstOrDefault(m => m.Title == "Kung Fu Panda");
            var kungFuPanda2 = context.Movies.FirstOrDefault(m => m.Title == "Kung Fu Panda 2");
            var iceAge = context.Movies.FirstOrDefault(m => m.Title == "Ice Age");
            var up = context.Movies.FirstOrDefault(m => m.Title == "Up");
            var shrek = context.Movies.FirstOrDefault(m => m.Title == "Shrek");
            var liloAndStitch = context.Movies.FirstOrDefault(m => m.Title == "Lilo & Stitch");
            var ratatouille = context.Movies.FirstOrDefault(m => m.Title == "Ratatouille");

            kungFuPanda1?.StreamingServices.Add(hbo);
            kungFuPanda1?.StreamingServices.Add(disney);

            kungFuPanda2?.StreamingServices.Add(hbo);
            kungFuPanda2?.StreamingServices.Add(disney);

            iceAge?.StreamingServices.Add(disney);
            iceAge?.StreamingServices.Add(hbo);
            iceAge?.StreamingServices.Add(appleTV);

            up?.StreamingServices.Add(disney);
            up?.StreamingServices.Add(appleTV);

            shrek?.StreamingServices.Add(hbo);
            shrek?.StreamingServices.Add(appleTV);

            liloAndStitch?.StreamingServices.Add(disney);

            ratatouille?.StreamingServices.Add(disney);
            ratatouille?.StreamingServices.Add(appleTV);

            wolfOfWallStreet?.StreamingServices.Add(appleTV);
            wolfOfWallStreet?.StreamingServices.Add(disney);

            trainingDay?.StreamingServices.Add(hbo);
            trainingDay?.StreamingServices.Add(appleTV);

            despereaux?.StreamingServices.Add(disney);

            moana2?.StreamingServices.Add(disney);
            moana2?.StreamingServices.Add(appleTV);

            cars?.StreamingServices.Add(disney);
            cars?.StreamingServices.Add(hbo);
            cars?.StreamingServices.Add(appleTV);

            johnWick?.StreamingServices.Add(appleTV);
            johnWick?.StreamingServices.Add(disney);

            deadpool?.StreamingServices.Add(disney);

            avengers?.StreamingServices.Add(disney);
            avengers?.StreamingServices.Add(hbo);

            godfather?.StreamingServices.Add(hbo);
            godfather?.StreamingServices.Add(appleTV);

            pulpFiction?.StreamingServices.Add(appleTV);

            scarface?.StreamingServices.Add(hbo);
            scarface?.StreamingServices.Add(appleTV);

            spiderman?.StreamingServices.Add(disney);
            spiderman?.StreamingServices.Add(hbo);
            spiderman?.StreamingServices.Add(appleTV);


            context.SaveChanges();
        }

        if (!context.WatchHistories.Any())
        {
            var watchedMovies = context.Movies.ToList();
            var histories = new List<WatchHistory>();

            if (demir != null)
            {
                var demirMovies = watchedMovies
                    .Where(m => m.Title == "John Wick" || m.Title == "Avengers" || m.Title == "Pulp Fiction" ||
                                m.Title == "Deadpool" || m.Title == "Cars")
                    .ToList();

                foreach (var movie in demirMovies)
                {
                    histories.Add(new WatchHistory
                    {
                        UserId = demir.Id,
                        MediaId = movie.Id,
                        WatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-demirMovies.IndexOf(movie) - 1)),
                        TimeLeftOf = movie.Duration - 20
                    });
                }
            }

            if (aiko != null)
            {
                var aikoMovies = watchedMovies
                    .Where(m => m.Title == "John Wick" || m.Title == "Deadpool" || m.Title == "Spiderman" ||
                                m.Title == "Pulp Fiction" || m.Title == "Cars")
                    .ToList();

                foreach (var movie in aikoMovies)
                {
                    histories.Add(new WatchHistory
                    {
                        UserId = aiko.Id,
                        MediaId = movie.Id,
                        WatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-aikoMovies.IndexOf(movie) - 2)),
                        TimeLeftOf = movie.Duration - 15
                    });
                }
            }

            if (michal != null)
            {
                var michalMovies = watchedMovies
                    .Where(m => m.Title == "Scarface" || m.Title == "God Father" || m.Title == "Cars")
                    .ToList();

                foreach (var movie in michalMovies)
                {
                    histories.Add(new WatchHistory
                    {
                        UserId = michal.Id,
                        MediaId = movie.Id,
                        WatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-michalMovies.IndexOf(movie) - 3)),
                        TimeLeftOf = movie.Duration - 30
                    });
                }
            }

            context.WatchHistories.AddRange(histories);
            context.SaveChanges();
        }

        if (!context.Reviews.Any())
        {
            var reviews = new List<Review>();
            var watchHistories = context.WatchHistories.Include(w => w.MediaContent).ToList();
            var allMovies = context.Movies.ToList();


            if (aiko != null)
            {
                var aikoTitles = new[] { "Deadpool", "Spiderman", "Cars"};
                foreach (var title in aikoTitles)
                {
                    var media = allMovies.FirstOrDefault(m => m.Title == title);
                    var history = watchHistories.FirstOrDefault(w => w.UserId == aiko.Id && w.MediaId == media.Id);
                    if (history != null && media != null)
                    {
                        reviews.Add(new Review
                        {
                            UserId = aiko.Id,
                            MediaId = media.Id,
                            Comment = title switch
                            {
                                "Deadpool" =>
                                    "Deadpool is a wild, hilarious ride with relentless action and humor that constantly breaks the fourth wall. Ryan Reynolds was born to play this role.",
                                "Spiderman" =>
                                    "A fresh take on Peter Parker’s journey. The high school drama blended well with superhero action, though I felt some character arcs were a bit rushed.",
                                "Cars" => "Ka-chow!",
                                _ => ""
                            },
                            MediaContent = media,
                            WatchHistory = history
                        });
                    }
                }
            }

            if (michal != null)
            {
                var michalTitles = new[] { "Scarface", "God Father", "Cars" };
                foreach (var title in michalTitles)
                {
                    var media = allMovies.FirstOrDefault(m => m.Title == title);
                    var history = watchHistories.FirstOrDefault(w => w.UserId == michal.Id && w.MediaId == media.Id);
                    if (history != null && media != null)
                    {
                        reviews.Add(new Review
                        {
                            UserId = michal.Id,
                            MediaId = media.Id,
                            Comment = title switch
                            {
                                "God Father" =>
                                    "The Godfather isn’t just a movie — it’s an experience. Rich characters, perfect pacing, and one of the best opening scenes in cinema history.",
                                "Scarface" =>
                                    "Scarface is raw and intense. Al Pacino's performance is unforgettable. The film’s rise-and-fall narrative is brutal and honest.",
                                "Cars" => "Never gets old",
                                _ => ""
                            },
                            MediaContent = media,
                            WatchHistory = history
                        });
                    }
                }
            }

            context.Reviews.AddRange(reviews);
            context.SaveChanges();
        }
    }
}