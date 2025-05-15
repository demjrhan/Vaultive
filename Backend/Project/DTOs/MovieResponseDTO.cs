namespace Project.DTOs;

public class MovieResponseDTO
{
    public MediaContentResponseDTO MediaContent { get; set; }
    public List<string> Genres { get; set; }
}