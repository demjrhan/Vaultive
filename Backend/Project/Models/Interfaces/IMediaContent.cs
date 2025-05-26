namespace Project.Models.Interfaces;

public interface IMediaContent
{
    int Id { get; set; }
    string Title { get; set; }
    string Description { get; set; }
    DateOnly ReleaseDate { get; set; }
    string OriginalLanguage { get; set; }
    string Country { get; set; }
    int Duration { get; set; }

    ICollection<Review> Reviews { get; }
    ICollection<WatchHistory> WatchHistories { get; }
    ICollection<StreamingService> StreamingServices { get; }

    SubtitleOption? SubtitleOption { get; set; }
    AudioOption? AudioOption { get; set; }

    /* Frontend visual properties */ 
    string? PosterImageName { get; set; }
    string YoutubeTrailerURL { get; set; }
}