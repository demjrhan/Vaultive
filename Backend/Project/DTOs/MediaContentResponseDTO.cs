
namespace Project.DTOs;

public class MediaContentResponseDTO
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string OriginalLanguage { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Duration { get; set; }
    public string TrailerId { get; set; }  = null!;
    public string PosterImage { get; set; }  = null!;
    
    public ICollection<StreamingServiceResponseDTO> StreamingServices { get; set; } = new HashSet<StreamingServiceResponseDTO>();

}