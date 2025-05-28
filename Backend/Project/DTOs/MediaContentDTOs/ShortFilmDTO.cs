namespace Project.DTOs.MediaContentDTOs;

public class ShortFilmDTO
{
    public MediaContentDetailedDTO MediaContentDetailed { get; set; }
    public ICollection<string> Genres { get; set; }
    public string SchoolName { get; set; } = null!;

}