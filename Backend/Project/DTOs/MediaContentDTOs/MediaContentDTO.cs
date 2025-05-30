namespace Project.DTOs.MediaContentDTOs;

public class MediaContentDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public string OriginalLanguage { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Duration { get; set; }
    
    public string State { get; set; } = null!;

}