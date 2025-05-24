namespace Project.DTOs.ReviewDTOs;

public class ReviewDTO
{
    public int Id { get; set; }
    public string MediaTitle { get; set; } = null!;
    public string Comment { get; set; } = null!;
    public string? Nickname { get; set; }
    public DateOnly WatchedOn { get; set; }
}