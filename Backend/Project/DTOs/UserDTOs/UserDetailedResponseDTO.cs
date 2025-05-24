using Project.DTOs.ReviewDTOs;
using Project.DTOs.SubscriptionDTOs;
using Project.DTOs.WatchHistoryDTOs;

namespace Project.DTOs.UserDTOs;

public class UserDetailedResponseDTO
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Nickname { get; set; }
    public string Country { get; set; }
    public string Status { get; set; }
    
    public IEnumerable<ReviewDTO> Reviews { get; set; }
    public ICollection<WatchHistoryResponseDTO> WatchHistories { get; set; }
    public ICollection<SubscriptionConfirmationDTO> Confirmations { get; set; }
}