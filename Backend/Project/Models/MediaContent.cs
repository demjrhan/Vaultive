
namespace Project.Models;

public abstract class MediaContent
{

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string OriginalLanguage { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Duration { get; set; }

    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    public ICollection<WatchHistory> WatchHistories { get; set; } = new HashSet<WatchHistory>();
    public ICollection<MediaContentStreamingService> MediaContentStreamingServices { get; set; } = new HashSet<MediaContentStreamingService>();
    public ICollection<StreamingService> StreamingServices { get; set; } = new HashSet<StreamingService>();

    public SubtitleOption SubtitleOption { get; set; } = null!;
    public int SubtitleOptionId { get; set; }
    
    public AudioOption AudioOption { get; set; } = null!;
    public int AudioOptionId { get; set; }
    
    
    /* properties for frontend visual */
    
    public string PosterImage { get; set; } = null!;
    public string BackgroundImage { get; set; } = null!;
    


}
