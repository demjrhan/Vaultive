namespace Project.DTOs.MediaContentDTOs;

public class DocumentaryResponseDTO
{
    public MediaContentResponseDTO MediaContentResponse { get; set; }
    public List<string> Topics { get; set; }
}