namespace Project.DTOs.MediaContentDTOs;

public class CreateMovieDTO
{
    public CreateMediaContentDTO MediaContent { get; set; }
    public ICollection<string> Genres { get; set; } 

}