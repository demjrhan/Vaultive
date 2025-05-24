namespace Project.DTOs.StreamingServiceDTOs;

public class StreamingServiceDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    public decimal DefaultPrice { get; set; }
    
    public string LogoImage { get; set; } = null!;
    public string WebsiteLink { get; set; } = null!;
}