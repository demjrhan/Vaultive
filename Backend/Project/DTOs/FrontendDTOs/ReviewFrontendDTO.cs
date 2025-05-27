namespace Project.DTOs.FrontendDTOs;

public class ReviewFrontendDTO
{
    public int Id { get; set; }
    public string? Comment { get; set; }
    public string? Nickname { get; set; }
    
    public DateOnly WatchedOn { get; set; }

}