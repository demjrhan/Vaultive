namespace Project.DTOs;

public class UserWithWatchHistoryDTO
{

    public UserResponseDTO User { get; set; }

    public IEnumerable<WatchHistoryResponseDTO> WatchHistory { get; set; } 

}