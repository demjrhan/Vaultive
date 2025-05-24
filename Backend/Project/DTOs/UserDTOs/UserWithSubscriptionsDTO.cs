using Project.DTOs.SubscriptionDTOs;

namespace Project.DTOs.UserDTOs;

public class UserWithSubscriptionsDTO
{
    public UserDTO User { get; set; }
    public IEnumerable<SubscriptionDTO> Subscriptions { get; set; } 
}