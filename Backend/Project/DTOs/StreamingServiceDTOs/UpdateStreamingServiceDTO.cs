namespace Project.DTOs.StreamingServiceDTOs;

public class UpdateStreamingServiceDTO
{
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal DefaultPrice { get; set; }
    public string LogoImage { get; set; } = null!;
    public string WebsiteLink { get; set; } = null!;
    
    public ICollection<int> SupportedMediaContents { get; set; }
}