namespace Project.Models;

public class StreamingService
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal DefaultPrice { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
    public ICollection<MediaContent> MediaContents { get; set; } = new HashSet<MediaContent>();
    
    /* properties for frontend visual */
    
    public string LogoImage { get; set; } = null!;
    public string WebsiteLink { get; set; } = null!;
}
