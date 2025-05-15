
namespace Project.DTOs;

public class MediaContentResponseDTO
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string OriginalLanguage { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Duration { get; set; }
    public ICollection<ReviewResponseDTO> Reviews { get; set; } = new HashSet<ReviewResponseDTO>();
    public double Rating { get; set; }
    public string Comment { get; set; } = null!;

}