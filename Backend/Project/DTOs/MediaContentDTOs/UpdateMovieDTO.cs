namespace Project.DTOs.MediaContentDTOs;

public class UpdateMovieDTO
{
    
    public UpdateMediaContentDTO MediaContent { get; set; } = new();

    public List<string> Genres { get; set; }  = new();

}