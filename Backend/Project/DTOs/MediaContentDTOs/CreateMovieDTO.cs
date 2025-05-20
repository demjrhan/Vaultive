namespace Project.DTOs.MediaContentDTOs;

public class CreateMovieDTO
{
    public CreateMediaContentDTO MediaContent { get; set; } = new();
    public HashSet<string> Genres { get; set; }  = new();

}