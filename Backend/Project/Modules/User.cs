using Project.Modules.Enumerations;

namespace Project.Modules;

public class User
{
    public int Id { get; set; }

    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Country { get; set; } = null!;
    public Status Status { get; set; }

    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    public ICollection<WatchHistory> WatchHistories { get; set; } = new HashSet<WatchHistory>();
    public ICollection<SubscriptionConfirmation> Confirmations { get; set; } = new HashSet<SubscriptionConfirmation>();
}
