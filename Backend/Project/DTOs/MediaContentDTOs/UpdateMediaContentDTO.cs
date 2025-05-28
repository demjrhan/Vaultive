using Project.DTOs.OptionDTOs;

namespace Project.DTOs.MediaContentDTOs;

public class UpdateMediaContentDTO
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }

    public string OriginalLanguage { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Duration { get; set; }
    public OptionDTO? AudioOption { get; set; }
    public OptionDTO? SubtitleOption { get; set; }
    
    public ICollection<int> StreamingServiceIds { get; set; }

    public string? PosterImageName { get; set; } = null;
    
    public string? YoutubeTrailerURL { get; set; }
}