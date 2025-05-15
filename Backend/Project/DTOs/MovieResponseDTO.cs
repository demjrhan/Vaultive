using Project.Models.Enumerations;

namespace Project.DTOs;

public class MovieResponseDTO
{
    public MediaContentResponseDTO MediaContent { get; set; }
    public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
}