namespace Project.DTOs.MediaContentDTOs;

public class DocumentaryResponseDTO
{
    public MediaContentDetailedResponseDTO MediaContentDetailedResponse { get; set; }
    public List<string> Topics { get; set; }
}