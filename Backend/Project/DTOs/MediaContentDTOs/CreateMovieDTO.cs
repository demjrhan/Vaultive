namespace Project.DTOs.MediaContentDTOs;

public class CreateMovieDTO
{
    public CreateMediaContentDTO MediaContent { get; set; } = new();
    public List<string> Genres { get; set; }  = new();

}