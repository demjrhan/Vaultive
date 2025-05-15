using Project.Models.Enumerations;
using Project.Models;

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
                    Firstname = "Alice", Lastname = "Brown", Nickname = "AliB", Email = "alice@example.com",
                    Country = "US", Status = Status.Normal
                },
                new()
                {
                    Firstname = "Bob", Lastname = "Smith", Nickname = "Bobby", Email = "bob@example.com",
                    Country = "UK", Status = Status.Student
                },
                new()
                {
                    Firstname = "Carol", Lastname = "Jones", Nickname = "CJ", Email = "carol@example.com",
                    Country = "CA", Status = Status.Elder
                },
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        if (!context.StreamingServices.Any())
        {
            var services = new List<StreamingService>
            {
                new() { Name = "Streamify", Country = "US", Description = "Popular streaming service" },
                new() { Name = "CineMax", Country = "UK", Description = "All the blockbusters" },
            };
            context.StreamingServices.AddRange(services);
            context.SaveChanges();
        }

        if (!context.Subscriptions.Any())
        {
            var subs = new List<Subscription>
            {
                new()
                {
                    DefaultPrice = 9.99,  StreamingServiceId = context.StreamingServices.First().Id
                },
                new()
                {
                    DefaultPrice = 24.99, 
                    StreamingServiceId = context.StreamingServices.Skip(1).First().Id
                },
                new()
                {
                    DefaultPrice = 5.99, StreamingServiceId = context.StreamingServices.First().Id
                },
            };
            context.Subscriptions.AddRange(subs);
            context.SaveChanges();
        }

        if (!context.SubscriptionConfirmations.Any())
        {
            var alice = context.Users.FirstOrDefault(u => u.Email == "alice@example.com");
            var bob = context.Users.FirstOrDefault(u => u.Email == "bob@example.com");
            var carol = context.Users.FirstOrDefault(u => u.Email == "carol@example.com");

            var subscriptions = context.Subscriptions.ToList();

            var confirmations = new List<SubscriptionConfirmation>();

            if (alice != null)
            {
                confirmations.Add(new SubscriptionConfirmation
                {
                    UserId = alice.Id,
                    SubscriptionId = subscriptions[0].Id,
                    PaymentMethod = "CreditCard",
                    StartTime = DateTime.UtcNow.AddDays(-5),
                    EndTime = DateTime.UtcNow.AddDays(25)
                });

                confirmations.Add(new SubscriptionConfirmation
                {
                    UserId = alice.Id,
                    SubscriptionId = subscriptions[1].Id,
                    PaymentMethod = "PayPal",
                    StartTime = DateTime.UtcNow.AddMonths(-1),
                    EndTime = DateTime.UtcNow.AddMonths(2)
                });
            }

            if (bob != null)
            {
                confirmations.Add(new SubscriptionConfirmation
                {
                    UserId = bob.Id,
                    SubscriptionId = subscriptions[2].Id,
                    PaymentMethod = "Voucher",
                    StartTime = DateTime.UtcNow.AddDays(-3),
                    EndTime = DateTime.UtcNow.AddDays(4)
                });
            }

            if (carol != null)
            {
                confirmations.Add(new SubscriptionConfirmation
                {
                    UserId = carol.Id,
                    SubscriptionId = subscriptions[1].Id,
                    PaymentMethod = "BankTransfer",
                    StartTime = DateTime.UtcNow.AddDays(-10),
                    EndTime = DateTime.UtcNow.AddDays(80)
                });
            }

            context.SubscriptionConfirmations.AddRange(confirmations);
            context.SaveChanges();
        }

        if (!context.Movies.Any())
        {
            var movie = new Movie
            {
                Title = "The Matrix",
                Description = "Neo discovers the truth about reality.",
                ReleaseDate = new DateTime(1999, 3, 31),
                OriginalLanguage = "English",
                Country = "USA",
                Duration = 136,
                Genres = new HashSet<Genre> { Genre.Action, Genre.SciFi },
                SubtitleOption = new SubtitleOption
                {
                    MediaTitle = "The Matrix",
                    SubtitleLanguages = new List<SubtitleLanguage>
                    {
                        new() { Language = "English" },
                        new() { Language = "German" }
                    }
                },
            };

            var movie2 = new Movie
            {
                Title = "Romantic Escape",
                Description = "A love story across continents.",
                ReleaseDate = new DateTime(2020, 2, 14),
                OriginalLanguage = "French",
                Country = "France",
                Duration = 112,
                Genres = new HashSet<Genre> { Genre.Romantic },
                SubtitleOption = new SubtitleOption
                {
                    MediaTitle = "Romantic Escape",
                    SubtitleLanguages = new List<SubtitleLanguage>
                    {
                        new() { Language = "English" }
                    }
                }
            };

            context.Movies.AddRange(movie, movie2);
            context.SaveChanges();
        }

        if (!context.WatchHistories.Any())
        {
            var alice = context.Users.FirstOrDefault(u => u.Email == "alice@example.com");
            var bob = context.Users.FirstOrDefault(u => u.Email == "bob@example.com");
            var matrix = context.Movies.FirstOrDefault(m => m.Title == "The Matrix");
            var romantic = context.Movies.FirstOrDefault(m => m.Title == "Romantic Escape");

            var watchHistories = new List<WatchHistory>();

            if (alice != null && matrix != null)
            {
                watchHistories.Add(new WatchHistory
                {
                    UserId = alice.Id,
                    MediaTitle = matrix.Title,
                    WatchDate = DateTime.UtcNow.AddDays(-2),
                    TimeLeftOf = 0
                });
            }

            if (bob != null && romantic != null)
            {
                watchHistories.Add(new WatchHistory
                {
                    UserId = bob.Id,
                    MediaTitle = romantic.Title,
                    WatchDate = DateTime.UtcNow.AddDays(-1),
                    TimeLeftOf = 10
                });
            }

            context.WatchHistories.AddRange(watchHistories);
            context.SaveChanges();
        }

        if (!context.Reviews.Any())
        {
            var matrixWH = context.WatchHistories
                .FirstOrDefault(w => w.MediaTitle == "The Matrix");

            var romanticWH = context.WatchHistories
                .FirstOrDefault(w => w.MediaTitle == "Romantic Escape");

            var reviews = new List<Review>();

            if (matrixWH != null)
            {
                reviews.Add(new Review
                {
                    UserId = matrixWH.UserId,
                    MediaTitle = matrixWH.MediaTitle,
                    Rating = 4.5,
                    Comment = "A mind-bending classic!"
                });
            }

            if (romanticWH != null)
            {
                reviews.Add(new Review
                {
                    UserId = romanticWH.UserId,
                    MediaTitle = romanticWH.MediaTitle,
                    Rating = 3.0,
                    Comment = "Heartfelt but slow-paced."
                });
            }

            context.Reviews.AddRange(reviews);
            context.SaveChanges();
        }
    }
}