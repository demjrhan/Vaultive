using Project.DTOs.WatchHistoryDTOs;

namespace Project.DTOs.UserDTOs;

public class UserWithWatchHistoryDTO
{

    public UserDTO User { get; set; }

    public IEnumerable<WatchHistoryDTO> WatchHistory { get; set; } 

}