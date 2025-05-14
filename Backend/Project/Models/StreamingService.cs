namespace Project.Models;

public class StreamingService
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
    public ICollection<MediaContentStreamingService> MediaContentStreamingServices { get; set; } = new HashSet<MediaContentStreamingService>();
}
