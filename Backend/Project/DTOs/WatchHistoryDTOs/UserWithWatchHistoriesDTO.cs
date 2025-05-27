using Project.DTOs.UserDTOs;

namespace Project.DTOs.WatchHistoryDTOs;

public class UserWithWatchHistoriesDTO
{
    
    public UserDTO User { get; set; }
    
    public ICollection<WatchHistoryDTO> WatchHistories { get; set; }

}