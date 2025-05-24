using Project.DTOs.SubscriptionDTOs;

namespace Project.DTOs.UserDTOs;

public class UserWithSubscriptionsDTO
{
    public UserResponseDTO User { get; set; }
    public IEnumerable<SubscriptionDTO> Subscriptions { get; set; } 
}