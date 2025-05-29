namespace Project.DTOs.MediaContentDTOs;

public class UpdateDocumentaryDTO
{
    public UpdateMediaContentDTO MediaContent { get; set; } =  new();
    public List<string> Topics { get; set; } = new();
}