using Project.DTOs.ReviewDTOs;

namespace Project.DTOs.UserDTOs;

public class UserWithReviewsDTO
{
    public UserResponseDTO User { get; set; }
    public IEnumerable<ReviewResponseDTO> Reviews { get; set; } 

}