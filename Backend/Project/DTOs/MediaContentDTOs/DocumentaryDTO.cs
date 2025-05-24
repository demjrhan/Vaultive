namespace Project.DTOs.MediaContentDTOs;

public class DocumentaryDTO
{
    public MediaContentDetailedDTO MediaContentDetailed { get; set; }
    public ICollection<string> Topics { get; set; }
}