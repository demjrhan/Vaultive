using Project.DTOs.UserDTOs;

namespace Project.DTOs.ReviewDTOs;

public class ReviewResponseDTO
{
    public int Id { get; set; }

    public double Rating { get; set; }
    public string Comment { get; set; } = null!;

    public UserResponseDTO User { get; set; }
}