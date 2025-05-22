namespace Project.DTOs.ReviewDTOs;

public class AddReviewDTO
{
    public int UserId { get; set; }
    public int MediaId { get; set; }
    public string Comment { get; set; } = null!;
}