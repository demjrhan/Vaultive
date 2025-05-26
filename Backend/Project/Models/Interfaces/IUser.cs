using Project.Models.Enumerations;

namespace Project.Models.Interfaces;

public interface IUser
{
    int Id { get; set; }

    string? Firstname { get; set; }
    string? Lastname { get; set; }
    string Nickname { get; set; }
    string Email { get; set; }
    string Country { get; set; }
    Status Status { get; set; }

    ICollection<Review> Reviews { get; }
    ICollection<WatchHistory> WatchHistories { get; }
    ICollection<SubscriptionConfirmation> Confirmations { get; }
}