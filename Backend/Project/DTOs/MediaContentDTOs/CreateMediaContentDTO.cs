using Project.Models.Enumerations;

namespace Project.DTOs.MediaContentDTOs;

public class CreateMediaContentDTO
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string OriginalLanguage { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Duration { get; set; }

    public List<string>? AudioOption { get; set; }
    public List<string>? SubtitleOption { get; set; }
    public List<int> StreamingServiceIds { get; set; } = new();


}