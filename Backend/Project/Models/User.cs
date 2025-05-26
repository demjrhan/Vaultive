using Project.Models.Enumerations;
using Project.Models.Interfaces;

namespace Project.Models;

public class User : IUser
{
    public int Id { get; set; }

    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Country { get; set; } = null!;
    public Status Status { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<WatchHistory> WatchHistories { get; set; } = new List<WatchHistory>();
    public ICollection<SubscriptionConfirmation> Confirmations { get; set; } = new List<SubscriptionConfirmation>();
}
