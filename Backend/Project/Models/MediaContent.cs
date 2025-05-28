
using Project.Models.Interfaces;

namespace Project.Models;

public abstract class MediaContent : IMediaContent
{

    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public string OriginalLanguage { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Duration { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<WatchHistory> WatchHistories { get; set; } = new List<WatchHistory>();
    public ICollection<StreamingService> StreamingServices { get; set; } = new List<StreamingService>();

    public SubtitleOption? SubtitleOption { get; set; }
    
    public AudioOption? AudioOption { get; set; }
    
    
    /* properties for frontend visual */

    public string? PosterImageName { get; set; } 
    public string? YoutubeTrailerURL { get; set; }
    


}
