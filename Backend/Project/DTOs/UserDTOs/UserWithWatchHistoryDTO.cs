using Project.DTOs.WatchHistoryDTOs;

namespace Project.DTOs.UserDTOs;

public class UserWithWatchHistoryDTO
{

    public UserResponseDTO User { get; set; }

    public IEnumerable<WatchHistoryResponseDTO> WatchHistory { get; set; } 

}