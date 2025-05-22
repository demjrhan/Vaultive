using Project.DTOs.ReviewDTOs;
using Project.DTOs.SubscriptionDTOs;
using Project.DTOs.WatchHistoryDTOs;

namespace Project.DTOs.UserDTOs;

public class UserDetailedResponseDTO
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Nickname { get; set; }
    public string Country { get; set; }
    public string Status { get; set; }
    
    public ICollection<ReviewResponseDTO> Reviews { get; set; } = new List<ReviewResponseDTO>();
    public ICollection<WatchHistoryResponseDTO> WatchHistories { get; set; } = new List<WatchHistoryResponseDTO>();
    public ICollection<SubscriptionConfirmationResponseDTO> Confirmations { get; set; } = new List<SubscriptionConfirmationResponseDTO>();
}